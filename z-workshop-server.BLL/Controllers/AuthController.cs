using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IJwtServices jwt, IUserService userService) : ControllerBase
{
    private readonly IJwtServices _jwt = jwt;
    private readonly IUserService _userService = userService;

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        if (HttpContext.Items["User"] == null)
            return Unauthorized();
        return Ok(new { user = HttpContext.Items["User"] });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.UserLogin(request);
        if (result.IsSuccess)
            return Ok(new { token = _jwt.GenerateToken(result.Data!) });
        return StatusCode(result.Code, result);
    }
}
