using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController(IShopService shopService) : ControllerBase
{
    protected readonly IShopService _shopService = shopService;

    [HttpGet("getAllPurchase")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> Get()
    {
        var result = await _shopService.GetPurchases();
        return StatusCode(result.Code, result);
    }

    [HttpPost("getCustomerPurchases/{id}")]
    [Authorize(Roles = "Admin, SuperAdmin, self")]
    public async Task<IActionResult> GetCustomerPurchases(string id)
    {
        var result = await _shopService.GetCustomerPurchases(id);
        return StatusCode(result.Code, result);
    }

    [HttpGet("getLib/{id}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetLib(string id)
    {
        var result = await _shopService.GetCustomerLibrary(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost("purchase/{id}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Purchase(string id)
    {
        var user = HttpContext.Items["User"] as UserDTO;
        var result = await _shopService.PurchaseProducts(id, user.UserId);
        return StatusCode(result.Code, result);
    }
}
