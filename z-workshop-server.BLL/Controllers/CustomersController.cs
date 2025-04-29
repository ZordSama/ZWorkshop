using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
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

        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCustomersWithUser()
        {
            var result = await _customerService.GetAllWithUser();

            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin, self")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var result = await _customerService.GetByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin, self")]
        public async Task<IActionResult> UpdateCustomer(
            string id,
            [FromBody] CustomerUpdateFormData customerUpdateFormData
        )
        {
            var result = await _customerService.UpdateCustomer(id, customerUpdateFormData);
            return StatusCode(result.Code, result);
        }

        [HttpGet("is-registered/mail")]
        [AllowAnonymous]
        public async Task<IActionResult> IsMailRegistered([FromQuery] string mail)
        {
            if (String.IsNullOrEmpty(mail))
                return BadRequest("Mail is required");

            var result = await _customerService.IsMailRegistered(mail);

            return StatusCode(result.Code, result);
        }

        [HttpGet("is-registered/phone")]
        [AllowAnonymous]
        public async Task<IActionResult> IsPhoneRegistered([FromQuery] string phone)
        {
            if (String.IsNullOrEmpty(phone))
                return BadRequest("Phone is required");

            var result = await _customerService.IsPhoneRegistered(phone);

            return StatusCode(result.Code, result);
        }
    }
}
