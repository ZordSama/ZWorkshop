using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public string? Roles { get; set; }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
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

            async Task<bool> isSelf()
            {
                if (!Roles!.Contains("self"))
                    return false;
                return await AuthAttrHelper.CheckSelfInAction(user.UserId, context.HttpContext);
            }
            if (!inRole && !await isSelf())
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = 403 };
                return;
            }
        }
    }
}
