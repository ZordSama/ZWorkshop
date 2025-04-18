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
    // [Authorize(Roles = "Admin, SuperAdmin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return StatusCode(result.Code, result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(
        [FromBody] CustomerRegisterRequest customerRegisterRequest
    )
    {
        var result = await _userService.UserRegisterAsync(customerRegisterRequest);
        if (result.IsSuccess)
            return Ok(result);
        return StatusCode(result.Code, result);
    }

    [HttpPost("employee-issue")]
    [AllowAnonymous]
    public async Task<IActionResult> EmployeeIssue(
        [FromBody] EmployeeIssueRequest employeeIssueRequest
    )
    {
        var result = await _userService.EmployeeIssueAsync(employeeIssueRequest);
        if (result.IsSuccess)
            return Ok(result);
        return StatusCode(result.Code, result);
    }
}
