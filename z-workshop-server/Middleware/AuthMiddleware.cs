using z_workshop_server.Data;
using z_workshop_server.Services;

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
        var _userService = context.RequestServices.GetRequiredService<UserService>();
        string? token = context
            .Request.Headers["Authorization"]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();

        string? userId = null;
        if (token != null)
            userId = jwt.ValidateToken(token);
        if (userId != null)
        {
            context.Items["User"] = _userService.GetByIdAsync(userId);
        }

        await _next(context);
    }
}
