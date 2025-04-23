using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Services;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(EmployeeService employeeService) : ControllerBase
{
    protected readonly EmployeeService _employeeService = employeeService;

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _employeeService.GetAllAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin, self")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _employeeService.GetByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpGet("is-registered/mail")]
    [AllowAnonymous]
    public async Task<IActionResult> IsMailRegistered(string mail)
    {
        var result = await _employeeService.IsMailRegistered(mail);
        return StatusCode(result.Code, result);
    }

    [HttpGet("is-registered/phone")]
    [AllowAnonymous]
    public async Task<IActionResult> IsPhoneRegistered(string phone)
    {
        var result = await _employeeService.IsPhoneRegistered(phone);
        return StatusCode(result.Code, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin, self")]
    public async Task<IActionResult> UpdateEmployee(
        string id,
        [FromBody] EmployeeUpdateFormData employeeUpdateFormData
    )
    {
        var result = await _employeeService.UpdateEmployee(id, employeeUpdateFormData);
        return StatusCode(result.Code, result);
    }
}
