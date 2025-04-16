using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface IUserSevice { }

public class UserService
{
    protected readonly IUserRepository _userRepository;
    protected readonly ICustomerRepository _customerRepository;
    protected readonly IEmployeeRepository _employeeRepository;
    protected readonly IMapper _mapper;
    protected readonly IWorker _worker;

    public UserService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        IWorker worker
    )
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _worker = worker;
    }

    public async Task<ZServiceResult<UserDTO>> GetByIdAsync(string id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return ZServiceResult<UserDTO>.Failure("User not found");

            return ZServiceResult<UserDTO>.Success("", _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            return ZServiceResult<UserDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<UserDTO>> GetByUserName(string userName)
    {
        try
        {
            var user = await _userRepository.GetByProperty(u => u.Username, userName);

            if (user == null)
                return ZServiceResult<UserDTO>.Failure("User not found");

            return ZServiceResult<UserDTO>.Success("", _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            return ZServiceResult<UserDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> UserRegisterAsync(
        CustomerRegisterRequest customerRegisterRequest
    )
    {
        try
        {
            var userDto = _mapper.Map<UserDTO>(customerRegisterRequest.UserFormData);
            userDto.UserId = Guid.NewGuid().ToString("N");
            userDto.Role = "Customer";

            var customerDto = _mapper.Map<CustomerDTO>(customerRegisterRequest.CustomerFormData);
            customerDto.CustomerId = Guid.NewGuid().ToString("N");
            customerDto.UserId = userDto.UserId;

            var user = _mapper.Map<User>(userDto);
            var customer = _mapper.Map<Customer>(customerDto);

            using (var transaction = await _worker.BeginTransactionAsync())
            {
                try
                {
                    await _userRepository.AddAsync(user);
                    await _customerRepository.AddAsync(customer);
                    await _worker.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ZServiceResult<string>.Success("user created successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ZServiceResult<string>.Failure(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> EmployeeIssueAsync(
        EmployeeIssueRequest employeeIssueRequest
    )
    {
        try
        {
            var user = _mapper.Map<User>(employeeIssueRequest.UserFormData);
            user.UserId = Guid.NewGuid().ToString("N");

            var employee = _mapper.Map<Employee>(employeeIssueRequest.EmployeeFormData);
            employee.EmployeeId = Guid.NewGuid().ToString("N");
            employee.UserId = user.UserId;

            using (var transaction = await _worker.BeginTransactionAsync())
            {
                try
                {
                    await _userRepository.AddAsync(user);
                    await _employeeRepository.AddAsync(employee);
                    await _worker.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ZServiceResult<string>.Success("user created successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ZServiceResult<string>.Failure(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
