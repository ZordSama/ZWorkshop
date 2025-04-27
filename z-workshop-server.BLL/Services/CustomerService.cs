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
        var customer = await _repository.GetByProperty(c => c.UserId, id);
        return customer != null
            ? ZServiceResult<CustomerDTO>.Success("", _mapper.Map<CustomerDTO>(customer))
            : ZServiceResult<CustomerDTO>.Failure("Khách hàng không tồn tại", 404);
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
