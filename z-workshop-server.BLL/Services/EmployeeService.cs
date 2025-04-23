using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IEmployeeService : IZBaseService<Employee, EmployeeDTO>
{
    Task<ZServiceResult<bool>> IsMailRegistered(string mail);
    Task<ZServiceResult<bool>> IsPhoneRegistered(string phone);
    Task<ZServiceResult<EmployeeDTO>> GetByUserId(string userId);
}

public class EmployeeService(IEmployeeRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Employee, EmployeeDTO>(repository, mapper, worker, "Employee"),
        IEmployeeService
{
    public async Task<ZServiceResult<bool>> IsMailRegistered(string mail)
    {
        var employee = await _repository.GetByProperty(c => c.Email, mail);
        return ZServiceResult<bool>.Success("", employee != null);
    }

    public async Task<ZServiceResult<bool>> IsPhoneRegistered(string phone)
    {
        var employee = await _repository.GetByProperty(c => c.Phone, phone);
        return ZServiceResult<bool>.Success("", employee != null);
    }

    public async Task<ZServiceResult<EmployeeDTO>> GetByUserId(string userId)
    {
        var employee = await _repository.GetByProperty(c => c.UserId, userId);
        return employee != null
            ? ZServiceResult<EmployeeDTO>.Success("", _mapper.Map<EmployeeDTO>(employee))
            : ZServiceResult<EmployeeDTO>.Failure("Employee not found", 404);
    }
}
