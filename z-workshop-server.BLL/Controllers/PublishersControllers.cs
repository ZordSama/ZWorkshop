using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
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

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _publisherService.GetByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, SuperAdmin")]
    // [AllowAnonymous]
    public async Task<IActionResult> Create(PublisherFormData publisherFormData)
    {
        var result = await _publisherService.CreatePublishertAsync(publisherFormData);
        return StatusCode(result.Code, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> Update(
        string id,
        PublisherUpdateFormData publisherUpdateFormData
    )
    {
        var result = await _publisherService.UpdatePublisherAsync(id, publisherUpdateFormData);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _publisherService.DeletePublisherAsync(id);
        return StatusCode(result.Code, result);
    }
}
