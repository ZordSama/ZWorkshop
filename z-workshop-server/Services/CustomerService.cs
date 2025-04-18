using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.DTOs;
using z_workshop_server.Models;
using z_workshop_server.Repositories;

namespace z_workshop_server.Services;

public interface ICustomerService
{
    Task<ZServiceResult<CustomerDTO>> GetByIdAsync(string id);
    Task<ZServiceResult<List<CustomerDTO>>> GetAllAsync();
    Task<ZServiceResult<CustomerDTO>> UpdateAsync(CustomerDTO customerDTO);
    Task<ZServiceResult<string>> DeleteAsync(string id);
}

public class CustomerService : ICustomerService
{
    protected readonly ICustomerRepository _customerRepository;
    protected readonly IMapper _mapper;

    private readonly IWorker _worker;

    public CustomerService(IMapper mapper, ICustomerRepository customerRepository, IWorker worker)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
        _worker = worker;
    }

    public async Task<ZServiceResult<CustomerDTO>> GetByIdAsync(string id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return ZServiceResult<CustomerDTO>.Failure("Customer not found", 404);

            return ZServiceResult<CustomerDTO>.Success(
                "Data dispached successfully",
                _mapper.Map<CustomerDTO>(customer)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<CustomerDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<CustomerDTO>>> GetAllAsync()
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync();
            return ZServiceResult<List<CustomerDTO>>.Success(
                "Data dispached successfully",
                _mapper.Map<List<CustomerDTO>>(customers)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<List<CustomerDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<CustomerDTO>> UpdateAsync(CustomerDTO customerDTO)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerDTO.CustomerId);
            if (customer == null)
                return ZServiceResult<CustomerDTO>.Failure("Customer not found", 404);

            customer = _mapper.Map<Customer>(customerDTO);
            _customerRepository.Update(customer);
            await _worker.SaveChangesAsync();
            return ZServiceResult<CustomerDTO>.Success(
                $"Updated customer {customer.CustomerId} successfully",
                _mapper.Map<CustomerDTO>(customer)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<CustomerDTO>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> DeleteAsync(string id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return ZServiceResult<string>.Failure("Customer not found", 404);

            _customerRepository.Delete(customer);
            await _worker.SaveChangesAsync();
            return ZServiceResult<string>.Success(
                $"Deleted User {customer.CustomerId} successfully"
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
