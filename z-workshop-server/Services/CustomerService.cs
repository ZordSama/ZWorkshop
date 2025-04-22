using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface ICustomerService : IZBaseService<Customer, CustomerDTO>
{
    Task<ZServiceResult<bool>> IsMailRegistered(string mail);
    Task<ZServiceResult<bool>> IsPhoneRegistered(string phone);
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
}
