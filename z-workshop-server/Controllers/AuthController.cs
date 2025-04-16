using Microsoft.AspNetCore.Mvc;
using z_workshop_server.DTOs;

namespace z_workshop_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtServices _jwt;
    private readonly UserService _userService;

    public AuthController(IJwtServices jwt, UserService userService)
    {
        _jwt = jwt;
        _userService = userService;
    }

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
        var user = await _userService.UserLogin(request);
        if (user == null)
            return Unauthorized("invalid username or password");
        return Ok(new { token = _jwt.GenerateToken(user) });
    }
}
