using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.Services;

namespace z_workshop_server.BLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerService.GetAllAsync();

            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var result = await _customerService.GetByIdAsync(id);

            return StatusCode(result.Code, result);
        }
    }
}
