using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    protected readonly IProductService _productService = productService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _productService.GetAllAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProduct(string id)
    {
        var result = await _productService.GetByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> CreateProduct(ProductFormData productFormData)
    {
        var user = HttpContext.Items["User"] as UserDTO;
        var result = await _productService.CreateProduct(productFormData, user!.UserId);
        return StatusCode(result.Code, result);
    }
}
