using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IUserService : IZBaseService<User, UserDTO>
{
    Task<ZServiceResult<UserDTO>> GetByUsername(string username);
    Task<ZServiceResult<UserDTO>> UserLogin(LoginRequest loginRequest);
    Task<ZServiceResult<string>> UserRegisterAsync(CustomerRegisterRequest customerRegisterRequest);
    Task<ZServiceResult<string>> EmployeeIssueAsync(EmployeeIssueRequest employeeIssueRequest);

    // Task<ZServiceResult<UserDTO>> UpdateUserAsync(UserUpdateRequest userUpdateRequest);
    Task<ZServiceResult<string>> UpdateUserAuthAsync(ChangePasswordRequest changePasswordRequest);
    Task<ZServiceResult<string>> ReIssueUserPassword(string UserId, string opRole);
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
            userAuthDto.UserId = "user." + Guid.NewGuid().ToString("N");
            userAuthDto.Role = "Customer";
            userAuthDto.Password = BCrypt.Net.BCrypt.HashPassword(userAuthDto.Password);

            var customerDto = _mapper.Map<CustomerDTO>(customerRegisterRequest.CustomerFormData);
            customerDto.CustomerId = "customer." + Guid.NewGuid().ToString("N");
            customerDto.UserId = userAuthDto.UserId;

            var user = _mapper.Map<User>(userAuthDto);
            var customer = _mapper.Map<Customer>(customerDto);

            using var transaction = await _worker.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(user);
                await _customerRepository.AddAsync(customer);
                await _worker.SaveChangesAsync();

                await transaction.CommitAsync();
                return ZServiceResult<string>.Success("user created successfully", default, 201);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ZServiceResult<string>.Failure(ex.Message);
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
            user.UserId = "user." + Guid.NewGuid().ToString("N");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var employee = _mapper.Map<Employee>(employeeIssueRequest.EmployeeFormData);
            employee.EmployeeId = "employee." + Guid.NewGuid().ToString("N");
            employee.UserId = user.UserId;

            using var transaction = await _worker.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(user);
                await _employeeRepository.AddAsync(employee);
                await _worker.SaveChangesAsync();

                await transaction.CommitAsync();
                return ZServiceResult<string>.Success("user created successfully", default, 201);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ZServiceResult<string>.Failure(ex.Message);
            }
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public override async Task<ZServiceResult<string>> DeleteAsync(params object[] keys)
    {
        var user = await _repository.GetByIdAsync(keys);
        if (user == null)
            return ZServiceResult<string>.Failure("User not found", 404);
        using var transaction = await _worker.BeginTransactionAsync();
        try
        {
            var employee = await _employeeRepository.GetByProperty(e => e.EmployeeId, keys[0]);
            if (employee != null)
                _employeeRepository.Delete(employee);

            var customer = await _customerRepository.GetByProperty(c => c.CustomerId, keys[0]);
            if (customer != null)
                _customerRepository.Delete(customer);

            _repository.Delete(user);
            await _worker.SaveChangesAsync();

            await transaction.CommitAsync();
            return ZServiceResult<string>.Success($"User {user.Username} deleted successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    // public async Task<ZServiceResult<UserDTO>> UpdateUserAsync(UserUpdateRequest userUpdateRequest)
    // {
    //     return await base.UpdateAsync(_mapper.Map<UserDTO>(userUpdateRequest));
    // }

    public async Task<ZServiceResult<string>> UpdateUserAuthAsync(
        ChangePasswordRequest changePasswordRequest
    )
    {
        try
        {
            var user = await _repository.GetByIdAsync(changePasswordRequest.UserId);

            if (user == null)
                return ZServiceResult<string>.Failure("User not found", 404);

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.Password))
                return ZServiceResult<string>.Failure("Old password is incorrect", 401);

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success("User updated successfully");
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> ReIssueUserPassword(string UserId, string opRole)
    {
        try
        {
            var user = await _repository.GetByIdAsync(UserId);

            if (user == null)
                return ZServiceResult<string>.Failure("User not found", 404);

            if (opRole == "Admin" && user.Role == "SuperAdmin")
                return ZServiceResult<string>.Failure(
                    "You are not authorized to perform this action!",
                    401
                );

            user.Password = BCrypt.Net.BCrypt.HashPassword("123456");
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success("User password reset successfully");
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    // public async Task<ZServiceResult<string>>
}
