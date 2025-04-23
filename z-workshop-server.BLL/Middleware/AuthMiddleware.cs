using z_workshop_server.BLL.Services;
using z_workshop_server.DAL.Data;

namespace z_workshop_server.BLL.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var jwt = context.RequestServices.GetRequiredService<IJwtServices>();
        var _userService = context.RequestServices.GetRequiredService<IUserService>();
        string? token = context
            .Request.Headers["Authorization"]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();

        string? userId = null;
        if (token != null)
            userId = jwt.ValidateToken(token);
        // Console.WriteLine(userId);
        if (userId != null)
        {
            var result = await _userService.GetByIdAsync(userId);
            var user = result.Data;
            context.Items["User"] = user;
        }

        await _next(context);
    }
}
