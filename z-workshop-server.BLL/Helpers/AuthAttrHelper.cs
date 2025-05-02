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

        // Console.WriteLine("Objtype:" + reqObjType);
        if (user == null)
            return false;

        if (reqObjType == "user")
            return userId == reqObjId;

        if (reqObjType == "customer")
        {
            var customer = (await _customerService.GetByUserId(userId)).Data;
            return customer != null && customer.CustomerId == reqObjId;
        }
        if (reqObjType == "employee")
        {
            var employee = (await _employeeService.GetByUserId(userId)).Data;
            return employee != null && employee.EmployeeId == reqObjId;
        }

        return false;
    }
}
