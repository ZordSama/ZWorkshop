using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using z_workshop_server.DTOs;
using z_workshop_server.Services;

namespace z_workshop_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, SuperAdmin")]
    // [AllowAnonymous]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllAsync();

        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> UpdateUser(
        string id,
        [FromBody] UserUpdateRequest userUpdateRequest
    )
    {
        if (id != userUpdateRequest.UserId)
            return BadRequest("Id does not match");

        var result = await _userService.UpdateUserAsync(userUpdateRequest);

        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userService.DeleteAsync(id);

        return StatusCode(result.Code, result);
    }

    [HttpPost("change-password/{id}")]
    [Authorize]
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
}
