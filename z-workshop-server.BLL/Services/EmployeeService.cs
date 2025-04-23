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
        try
        {
            var employee = await _repository.GetByProperty(c => c.Email, mail);
            return ZServiceResult<bool>.Success("", employee != null);
        }
        catch (Exception ex)
        {
            return ZServiceResult<bool>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<bool>> IsPhoneRegistered(string phone)
    {
        try
        {
            var employee = await _repository.GetByProperty(c => c.Phone, phone);
            return ZServiceResult<bool>.Success("", employee != null);
        }
        catch (Exception ex)
        {
            return ZServiceResult<bool>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<EmployeeDTO>> GetByUserId(string userId)
    {
        var employee = await _repository.GetByProperty(c => c.UserId, userId);
        return employee != null
            ? ZServiceResult<EmployeeDTO>.Success("", _mapper.Map<EmployeeDTO>(employee))
            : ZServiceResult<EmployeeDTO>.Failure("Employee not found", 404);
    }

    public async Task<ZServiceResult<EmployeeDTO>> UpdateEmployee(
        string employeeId,
        EmployeeUpdateFormData employeeUpdateFormData
    )
    {
        try
        {
            if (employeeId != employeeUpdateFormData.EmployeeId)
                return ZServiceResult<EmployeeDTO>.Failure("Employee id mismatch", 400);

            var employee = await _repository.GetByIdAsync(employeeId);
            if (employee == null)
                return ZServiceResult<EmployeeDTO>.Failure("Employee not found", 404);

            _mapper.Map(employeeUpdateFormData, employee);
            _repository.Update(employee);
            await _worker.SaveChangesAsync();

            return ZServiceResult<EmployeeDTO>.Success("", _mapper.Map<EmployeeDTO>(employee));
        }
        catch (Exception ex)
        {
            return ZServiceResult<EmployeeDTO>.Failure(ex.Message);
        }
    }
}
