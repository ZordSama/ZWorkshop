using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using z_workshop_server.DTOs;

namespace z_workshop_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UsersController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public IActionResult GetUsers()
    {
        return Ok(_userService.GetAll());
    }

    [HttpPost("register")]
    // [Authorize(Roles = "Admin, SuperAdmin")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(
        [FromBody] CustomerRegisterRequest customerRegisterRequest
    )
    {
        try
        {
            if (
                await _userService.GetByUsername(customerRegisterRequest.UserFormData.Username)
                != null
            )
                return BadRequest("Username already exists");

            UserAuthDTO userDTO = _mapper.Map<UserAuthDTO>(customerRegisterRequest.UserFormData);

            userDTO.UserId = Guid.NewGuid().ToString("N");
            userDTO.CreatedAt = DateTime.UtcNow;
            userDTO.LastUpdate = DateTime.UtcNow;
            userDTO.Role = "Customer";

            CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(
                customerRegisterRequest.CustomerFormData
            );
            customerDTO.CustomerId = Guid.NewGuid().ToString("N");
            customerDTO.UserId = userDTO.UserId;
            customerDTO.CreatedAt = DateTime.UtcNow;
            customerDTO.LastUpdate = DateTime.UtcNow;
            customerDTO.Status = 0;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return StatusCode(500);
    }

    // [HttpPost("issueAccount")]
    // // [Authorize(Roles = "Admin, SuperAdmin")]
    // [AllowAnonymous]
    // public async Task<IActionResult> IssueAccount(
    //     [FromBody] UserRegisterRequest employeeIssueRequest
    // )
    // {
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);
    //     ZActionResult result = await _userService.Create();
    //     if (result.Success)
    //         return StatusCode(201, result);
    //     return StatusCode(500, result);
    // }
}
