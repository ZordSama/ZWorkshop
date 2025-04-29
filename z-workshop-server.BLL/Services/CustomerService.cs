using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface ICustomerService : IZBaseService<Customer, CustomerDTO>
{
    Task<ZServiceResult<bool>> IsMailRegistered(string mail);
    Task<ZServiceResult<bool>> IsPhoneRegistered(string phone);
    Task<ZServiceResult<CustomerDTO>> GetByUserId(string id);
    Task<ZServiceResult<CustomerWithUserDTO>> GetWithUserById(string id);
    Task<ZServiceResult<List<CustomerWithUserDTO>>> GetAllWithUser();
    Task<ZServiceResult<CustomerDTO>> UpdateCustomer(
        string customerId,
        CustomerUpdateFormData customerUpdateFormData
    );
}

public class CustomerService(ICustomerRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Customer, CustomerDTO>(repository, mapper, worker, "Khách hàng"),
        ICustomerService
{
    public async Task<ZServiceResult<bool>> IsMailRegistered(string mail)
    {
        try
        {
            var customer = await _repository.GetByProperty(c => c.Email, mail);
            return ZServiceResult<bool>.Success("", customer != null);
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
            var customer = await _repository.GetByProperty(c => c.Phone, phone);
            return ZServiceResult<bool>.Success("", customer != null);
        }
        catch (Exception ex)
        {
            return ZServiceResult<bool>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<CustomerDTO>> GetByUserId(string id)
    {
        try
        {
            var customer = await _repository.GetByProperty(c => c.UserId, id);
            return customer != null
                ? ZServiceResult<CustomerDTO>.Success("", _mapper.Map<CustomerDTO>(customer))
                : ZServiceResult<CustomerDTO>.Failure("Khách hàng không tồn tại", 404);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<CustomerDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<CustomerWithUserDTO>> GetWithUserById(string id)
    {
        try
        {
            var customer = await _repository.GetByIdWithIncludesAsync([id], c => c.User);
            if (customer == null)
                return ZServiceResult<CustomerWithUserDTO>.Failure("Khách hàng không tồn tại", 404);
            CustomerWithUserDTO customerWithUserDTO = new CustomerWithUserDTO();
            customerWithUserDTO.CustomerDto = _mapper.Map<CustomerDTO>(customer);
            customerWithUserDTO.UserDto = _mapper.Map<UserDTO>(customer.User);
            return ZServiceResult<CustomerWithUserDTO>.Success("", customerWithUserDTO);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<CustomerWithUserDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<CustomerWithUserDTO>>> GetAllWithUser()
    {
        try
        {
            var customers = await _repository.GetAllWithIncludesAsync(c => c.User);
            List<CustomerWithUserDTO> customerWithUserDTOs = new List<CustomerWithUserDTO>();
            foreach (var customer in customers)
            {
                CustomerWithUserDTO customerWithUserDTO = new CustomerWithUserDTO();
                customerWithUserDTO.CustomerDto = _mapper.Map<CustomerDTO>(customer);
                customerWithUserDTO.UserDto = _mapper.Map<UserDTO>(customer.User);
                customerWithUserDTOs.Add(customerWithUserDTO);
            }
            return ZServiceResult<List<CustomerWithUserDTO>>.Success("", customerWithUserDTOs);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<CustomerWithUserDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<CustomerDTO>> UpdateCustomer(
        string customerId,
        CustomerUpdateFormData customerUpdateFormData
    )
    {
        try
        {
            if (customerId != customerUpdateFormData.CustomerId)
                return ZServiceResult<CustomerDTO>.Failure("Mã khách hàng không khớp", 400);

            return await base.UpdateAsync(
                _mapper.Map<CustomerDTO>(customerUpdateFormData),
                customerId
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<CustomerDTO>.Failure(ex.Message);
        }
    }
}
