using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Helpers;

public class AuthAttrHelper
{
    public static async Task<bool> CheckSelfInAction(string userId, HttpContext context)
    {
        string reqObjId = context.Request.Path.Value!.Split('/').Last();
        string reqObjType = reqObjId.Split('.')[0];

        var _userService = context.RequestServices.GetService<IUserService>();
        var _employeeService = context.RequestServices.GetRequiredService<IEmployeeService>();
        var _customerService = context.RequestServices.GetRequiredService<ICustomerService>();

        var user = await _userService!.GetByIdAsync(userId);

        if (user == null)
            return false;

        if (reqObjType == "user")
            return userId == reqObjId;

        if (reqObjId == "customer")
            return (await _customerService.GetByUserId(userId)).IsSuccess;

        if (reqObjId == "employee")
            return (await _employeeService.GetByUserId(userId))!.IsSuccess;

        return false;
    }
}
