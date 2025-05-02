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
    Task<ZServiceResult<EmployeeWithUserDTO>> GetWithUserById(string userId);
    Task<ZServiceResult<List<EmployeeWithUserDTO>>> GetAllWithUser();
    Task<ZServiceResult<EmployeeDTO>> UpdateEmployee(
        string employeeId,
        EmployeeUpdateFormData employeeUpdateFormData
    );
}

public class EmployeeService(IEmployeeRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Employee, EmployeeDTO>(repository, mapper, worker, "Nhân viên"),
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
            : ZServiceResult<EmployeeDTO>.Failure("Nhân viên không tồn tại", 404);
    }

    public async Task<ZServiceResult<EmployeeWithUserDTO>> GetWithUserById(string userId)
    {
        try
        {
            var employee = await _repository.GetByIdWithIncludesAsync([userId], e => e.User);
            if (employee == null)
                return ZServiceResult<EmployeeWithUserDTO>.Failure("Nhân viên không tồn tại", 404);
            EmployeeWithUserDTO employeeWithUserDTO = new();
            employeeWithUserDTO.EmployeeDto = _mapper.Map<EmployeeDTO>(employee);
            employeeWithUserDTO.UserDto = _mapper.Map<UserDTO>(employee.User);
            return ZServiceResult<EmployeeWithUserDTO>.Success("", employeeWithUserDTO);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<EmployeeWithUserDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<EmployeeWithUserDTO>>> GetAllWithUser()
    {
        try
        {
            var employees = await _repository.GetAllWithIncludesAsync(e => e.User);
            List<EmployeeWithUserDTO> employeeWithUserDTOs = new();
            foreach (var employee in employees)
            {
                EmployeeWithUserDTO employeeWithUserDTO = new();
                employeeWithUserDTO.EmployeeDto = _mapper.Map<EmployeeDTO>(employee);
                employeeWithUserDTO.UserDto = _mapper.Map<UserDTO>(employee.User);
                employeeWithUserDTOs.Add(employeeWithUserDTO);
            }
            return ZServiceResult<List<EmployeeWithUserDTO>>.Success("", employeeWithUserDTOs);
        }
        catch (Exception ex)
        {
            return ZServiceResult<List<EmployeeWithUserDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<EmployeeDTO>> UpdateEmployee(
        string employeeId,
        EmployeeUpdateFormData employeeUpdateFormData
    )
    {
        try
        {
            if (employeeId != employeeUpdateFormData.EmployeeId)
                return ZServiceResult<EmployeeDTO>.Failure("Mã nhân viên không khớp", 400);

            return await base.UpdateAsync(
                _mapper.Map<EmployeeDTO>(employeeUpdateFormData),
                employeeId
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<EmployeeDTO>.Failure(ex.Message);
        }
    }
}
