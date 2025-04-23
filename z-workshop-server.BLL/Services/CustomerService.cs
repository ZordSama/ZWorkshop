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
}

public class CustomerService(ICustomerRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Customer, CustomerDTO>(repository, mapper, worker, "Customer"),
        ICustomerService
{
    public async Task<ZServiceResult<bool>> IsMailRegistered(string mail)
    {
        var customer = await _repository.GetByProperty(c => c.Email, mail);
        return ZServiceResult<bool>.Success("", customer != null);
    }

    public async Task<ZServiceResult<bool>> IsPhoneRegistered(string phone)
    {
        var customer = await _repository.GetByProperty(c => c.Phone, phone);
        return ZServiceResult<bool>.Success("", customer != null);
    }

    public async Task<ZServiceResult<CustomerDTO>> GetByUserId(string id)
    {
        var customer = await _repository.GetByProperty(c => c.UserId, id);
        return customer != null
            ? ZServiceResult<CustomerDTO>.Success("", _mapper.Map<CustomerDTO>(customer))
            : ZServiceResult<CustomerDTO>.Failure("Customer not found", 404);
    }
}
