using z_workshop_server.Data;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AppDbContext db, IJwtServices jwt)
    {
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
            context.Items["User"] = db.Users.FirstOrDefault(u => u.Id == userId)!;
        }

        await _next(context);
    }
}
