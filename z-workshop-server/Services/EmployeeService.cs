using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface IEmployeeService
{
    Task<ZServiceResult<EmployeeDTO>> GetByIdAsync(string id);
    Task<ZServiceResult<List<EmployeeDTO>>> GetAllAsync();
    Task<ZServiceResult<EmployeeDTO>> UpdateAsync(EmployeeDTO employeeDTO);
    Task<ZServiceResult<string>> DeleteAsync(string id);
}

public class EmployeeService : IEmployeeService
{
    protected readonly IEmployeeRepository _employeeRepository;
    protected readonly IMapper _mapper;
    protected readonly IWorker _worker;

    public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IWorker worker)
    {
        _mapper = mapper;
        _worker = worker;
        _employeeRepository = employeeRepository;
    }

    public async Task<ZServiceResult<EmployeeDTO>> GetByIdAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return ZServiceResult<EmployeeDTO>.Failure("Employee not found", 404);
            return ZServiceResult<EmployeeDTO>.Success(
                "Data dispached successfully",
                _mapper.Map<EmployeeDTO>(employee)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<EmployeeDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<EmployeeDTO>>> GetAllAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllAsync();
            return ZServiceResult<List<EmployeeDTO>>.Success(
                "Data dispached successfully",
                _mapper.Map<List<EmployeeDTO>>(employees)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<List<EmployeeDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<EmployeeDTO>> UpdateAsync(EmployeeDTO employeeDTO)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeDTO.EmployeeId);
            if (employee == null)
                return ZServiceResult<EmployeeDTO>.Failure("Employee not found", 404);
            employee = _mapper.Map<Employee>(employeeDTO);
            _employeeRepository.Update(employee);
            await _worker.SaveChangesAsync();
            return ZServiceResult<EmployeeDTO>.Success(
                $"Updated employee {employeeDTO.EmployeeId} successfully",
                _mapper.Map<EmployeeDTO>(employee)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<EmployeeDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> DeleteAsync(string id)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return ZServiceResult<string>.Failure("Employee not found", 404);
            _employeeRepository.Delete(employee);
            await _worker.SaveChangesAsync();
            return ZServiceResult<string>.Success($"Deleted employee {id} successfully");
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
