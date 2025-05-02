using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController(IPublisherService publisherService) : ControllerBase
{
    protected readonly IPublisherService _publisherService = publisherService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _publisherService.GetAllAsync();
        return StatusCode(result.Code, result);
    }
}
