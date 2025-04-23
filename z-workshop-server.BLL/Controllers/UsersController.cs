using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllAsync();

        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin, self")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _userService.GetByIdAsync(id);

        return StatusCode(result.Code, result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(
        [FromBody] CustomerRegisterRequest customerRegisterRequest
    )
    {
        var result = await _userService.UserRegisterAsync(customerRegisterRequest);

        return StatusCode(result.Code, result);
    }

    [HttpPost("employee-issue")]
    [AllowAnonymous]
    public async Task<IActionResult> EmployeeIssue(
        [FromBody] EmployeeIssueRequest employeeIssueRequest
    )
    {
        var result = await _userService.EmployeeIssueAsync(employeeIssueRequest);

        return StatusCode(result.Code, result);
    }

    // [HttpPut("{id}")]
    // [Authorize(Roles = "Admin, SuperAdmin, self")]
    // public async Task<IActionResult> UpdateUser(
    //     string id,
    //     [FromBody] UserUpdateRequest userUpdateRequest
    // )
    // {
    //     if (id != userUpdateRequest.UserId)
    //         return BadRequest("Id does not match");

    //     var result = await _userService.UpdateUserAsync(userUpdateRequest);

    //     return StatusCode(result.Code, result);
    // }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userService.DeleteAsync(id);

        return StatusCode(result.Code, result);
    }

    [HttpPatch("change-password/{id}")]
    [Authorize(Roles = "self")]
    public async Task<IActionResult> ChangePassword(
        string id,
        [FromBody] ChangePasswordRequest changePasswordRequest
    )
    {
        if (id != changePasswordRequest.UserId)
            return BadRequest("Id does not match");

        var result = await _userService.UpdateUserAuthAsync(changePasswordRequest);

        return StatusCode(result.Code, result);
    }

    [HttpPatch("re-issue-pwd/{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> ReIssuePwd(string id)
    {
        var user = HttpContext.Items["User"] as UserDTO;
        var result = await _userService.ReIssueUserPassword(id, user!.Role);

        return StatusCode(result.Code, result);
    }
}
