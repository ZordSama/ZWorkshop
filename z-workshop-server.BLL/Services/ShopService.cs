using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IShopService
{
    Task<ZServiceResult<string>> PurchaseProducts(string productId, string userId);
    Task<ZServiceResult<List<Purchase>>> GetPurchases();
    Task<ZServiceResult<List<Purchase>>> GetCustomerPurchases(string userId);
    Task<ZServiceResult<List<ProductDTO>>> GetCustomerLibrary(string userID);
}

public class ShopService : IShopService
{
    protected readonly IPurchaseRepository _purchaseRepository;
    protected readonly IProductRepository _productRepository;
    protected readonly IPurchaseDetailRepository _purchaseDetailRepository;
    protected readonly ILibraryRepository _libraryRepository;
    protected readonly ICustomerRepository _customerRepository;
    protected readonly IMapper _mapper;
    protected readonly IWorker _worker;

    public ShopService(
        IPurchaseRepository purchaseRepository,
        IProductRepository productRepository,
        IPurchaseDetailRepository purchaseDetailRepository,
        ILibraryRepository libraryRepository,
        ICustomerRepository customerRepository,
        IMapper mapper,
        IWorker worker
    )
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
        _purchaseDetailRepository = purchaseDetailRepository;
        _libraryRepository = libraryRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _worker = worker;
    }

    public async Task<ZServiceResult<string>> PurchaseProducts(string productId, string userId)
    {
        try
        {
            var customer = await _customerRepository.GetByProperty(c => c.UserId, userId);
            if (customer == null)
                return ZServiceResult<string>.Failure("Người dùng không hợp lệ", 400);
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return ZServiceResult<string>.Failure("Sản phẩm không tồn tại", 404);

            using var transaction = await _worker.BeginTransactionAsync();
            try
            {
                var purchase = new Purchase();
                purchase.PurchaseId = $"purchase.{Guid.NewGuid():N}";
                purchase.CustomerId = customer.CustomerId;
                purchase.Status = 0; //Open

                var purchaseDetail = new PurchaseDetail();
                purchaseDetail.PurchaseId = purchase.PurchaseId;
                purchaseDetail.ProductId = productId;
                purchaseDetail.UnitPrice = product.Price;

                await _purchaseRepository.AddAsync(purchase);
                await _purchaseDetailRepository.AddAsync(purchaseDetail);
                await _libraryRepository.AddAsync(customer.CustomerId, productId);

                await transaction.CommitAsync();
                return ZServiceResult<string>.Success();
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return ZServiceResult<string>.Failure(ex.Message);
            }
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<Purchase>>> GetPurchases()
    {
        try
        {
            var purchases = await _purchaseRepository.GetAllAsync();

            return ZServiceResult<List<Purchase>>.Success("Truyền dữ liệu hoàn tất", purchases);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<Purchase>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<Purchase>>> GetCustomerPurchases(string userId)
    {
        try
        {
            var customer = await _customerRepository.GetByProperty(c => c.UserId, userId);
            if (customer == null)
                return ZServiceResult<List<Purchase>>.Failure("Người dùng không hợp lệ", 400);
            var purchases = await _purchaseRepository.GetAllByProperty(
                p => p.CustomerId,
                customer.CustomerId
            );
            return ZServiceResult<List<Purchase>>.Success("Truyền dữ liệu hoàn tất", purchases);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<Purchase>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<ProductDTO>>> GetCustomerLibrary(string userID)
    {
        try
        {
            var customer = await _customerRepository.GetByProperty(c => c.UserId, userID);
            if (customer == null)
                return ZServiceResult<List<ProductDTO>>.Failure("Người dùng không hợp lệ", 400);

            var products = await _libraryRepository.GetProductsAsync(customer.CustomerId);
            return ZServiceResult<List<ProductDTO>>.Success(
                "Truyền dữ liệu hoàn tất",
                _mapper.Map<List<ProductDTO>>(products)
            );
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<ProductDTO>>.Failure(ex.Message);
        }
    }
}
