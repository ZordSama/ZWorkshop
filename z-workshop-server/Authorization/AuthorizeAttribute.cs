using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using z_workshop_server.DTOs;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string? Roles { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context
            .ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>()
            .Any();
        if (allowAnonymous)
            return;

        var user = context.HttpContext.Items["User"] as UserDTO;
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = 401 };
            return;
        }
        if (!string.IsNullOrWhiteSpace(Roles))
        {
            bool inRole = Roles.Contains(user!.Role);
            bool self =
                Roles.Contains("self")
                && user.UserId.ToString() == context.HttpContext.Request.Query["id"];

            if (!inRole && !self)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = 403 };
                return;
            }
        }
    }
}
