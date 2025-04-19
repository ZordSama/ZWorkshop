using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface IUserService : IZBaseService<User, UserDTO>
{
    Task<ZServiceResult<UserDTO>> GetByUsername(string username);
    Task<ZServiceResult<UserDTO>> UserLogin(LoginRequest loginRequest);
    Task<ZServiceResult<string>> UserRegisterAsync(CustomerRegisterRequest customerRegisterRequest);
    Task<ZServiceResult<string>> EmployeeIssueAsync(EmployeeIssueRequest employeeIssueRequest);
}

public class UserService : ZBaseService<User, UserDTO>, IUserService
{
    protected readonly ICustomerRepository _customerRepository;
    protected readonly IEmployeeRepository _employeeRepository;

    public UserService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        IWorker worker
    )
        : base(userRepository, mapper, worker, "User")
    {
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<ZServiceResult<UserDTO>> GetByUsername(string userName)
    {
        try
        {
            var user = await _repository.GetByProperty(u => u.Username, userName);

            if (user == null)
                return ZServiceResult<UserDTO>.Failure("User not found", 404);

            return ZServiceResult<UserDTO>.Success("", _mapper.Map<UserDTO>(user));
        }
        catch (Exception ex)
        {
            return ZServiceResult<UserDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<UserDTO>> UserLogin(LoginRequest loginRequest)
    {
        try
        {
            var user = await _repository.GetByProperty(u => u.Username, loginRequest.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                return ZServiceResult<UserDTO>.Success(
                    "User login successfully",
                    _mapper.Map<UserDTO>(user)
                );
            return ZServiceResult<UserDTO>.Failure("Username or password is incorrect", 401);
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
            var userAuthDto = _mapper.Map<UserAuthDTO>(customerRegisterRequest.UserFormData);
            userAuthDto.UserId = Guid.NewGuid().ToString("N");
            userAuthDto.Role = "Customer";
            userAuthDto.Password = BCrypt.Net.BCrypt.HashPassword(userAuthDto.Password);

            var customerDto = _mapper.Map<CustomerDTO>(customerRegisterRequest.CustomerFormData);
            customerDto.CustomerId = Guid.NewGuid().ToString("N");
            customerDto.UserId = userAuthDto.UserId;

            var user = _mapper.Map<User>(userAuthDto);
            var customer = _mapper.Map<Customer>(customerDto);

            using (var transaction = await _worker.BeginTransactionAsync())
            {
                try
                {
                    await _repository.AddAsync(user);
                    await _customerRepository.AddAsync(customer);
                    await _worker.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ZServiceResult<string>.Success(
                        "user created successfully",
                        default,
                        201
                    );
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
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var employee = _mapper.Map<Employee>(employeeIssueRequest.EmployeeFormData);
            employee.EmployeeId = Guid.NewGuid().ToString("N");
            employee.UserId = user.UserId;

            using (var transaction = await _worker.BeginTransactionAsync())
            {
                try
                {
                    await _repository.AddAsync(user);
                    await _employeeRepository.AddAsync(employee);
                    await _worker.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ZServiceResult<string>.Success(
                        "user created successfully",
                        default,
                        201
                    );
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
