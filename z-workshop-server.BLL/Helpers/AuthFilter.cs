using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace z_workshop_server.BLL.Helpers;

public class AuthFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var allowAnonymous = context
            .MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Any();

        if (allowAnonymous)
        {
            operation.Security = null;
        }
    }
}
