using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IShopService
{
    Task<ZServiceResult<string>> PurchaseProducts(string productId, string userId);
    Task<ZServiceResult<List<PurchaseDTO>>> GetPurchases();
    Task<ZServiceResult<List<PurchaseDTO>>> GetCustomerPurchases(string userId);
    Task<ZServiceResult<List<ProductDTO>>> GetCustomerLibrary(string userID);
}

public class ShopService : IShopService
{
    protected readonly IPurchaseRepository _purchaseRepository;
    protected readonly IProductRepository _productRepository;
    protected readonly IPurchaseDetailRepository _purchaseDetailRepository;
    protected readonly IPublisherRepository _publisherRepository;
    protected readonly ILibraryRepository _libraryRepository;
    protected readonly ICustomerRepository _customerRepository;
    protected readonly IMapper _mapper;
    protected readonly IWorker _worker;

    public ShopService(
        IPurchaseRepository purchaseRepository,
        IProductRepository productRepository,
        IPurchaseDetailRepository purchaseDetailRepository,
        IPublisherRepository publisherRepository,
        ILibraryRepository libraryRepository,
        ICustomerRepository customerRepository,
        IMapper mapper,
        IWorker worker
    )
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
        _purchaseDetailRepository = purchaseDetailRepository;
        _publisherRepository = publisherRepository;
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
                purchase.Status = 1; //Close
                purchase.CloseAt = DateTime.Now;

                var purchaseDetail = new PurchaseDetail();
                purchaseDetail.PurchaseId = purchase.PurchaseId;
                purchaseDetail.ProductId = productId;
                purchaseDetail.UnitPrice = product.Price;

                await _purchaseRepository.AddAsync(purchase);
                await _purchaseDetailRepository.AddAsync(purchaseDetail);
                await _libraryRepository.AddAsync(customer.CustomerId, productId);

                await _worker.SaveChangesAsync();
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

    public async Task<ZServiceResult<List<PurchaseDTO>>> GetPurchases()
    {
        try
        {
            var purchases = await _purchaseRepository.GetAllAsync();
            var purchaseDtos = new List<PurchaseDTO>();
            var purchaseDetails = await _purchaseDetailRepository.GetAllAsync();

            foreach (var purchase in purchases)
            {
                var purchaseDto = new PurchaseDTO();
                var thisPurchaseDetail = purchaseDetails.FirstOrDefault(p =>
                    p.PurchaseId == purchase.PurchaseId
                );
                if (thisPurchaseDetail == null)
                    return ZServiceResult<List<PurchaseDTO>>.Failure(
                        "Không tìm thấy chi tiết đơn hàng",
                        404
                    );
                var product = await _productRepository.GetByIdAsync(thisPurchaseDetail.ProductId);
                var customer = await _customerRepository.GetByIdAsync(purchase.CustomerId);
                if (product == null || customer == null)
                    return ZServiceResult<List<PurchaseDTO>>.Failure(
                        "Không tìm thấy sản phẩm hoặc khách hàng",
                        404
                    );

                purchaseDto.PurchaseId = purchase.PurchaseId;

                purchaseDto.CustomerId = purchase.CustomerId;
                purchaseDto.CustomerFullname = customer.Fullname;

                purchaseDto.ProductId = thisPurchaseDetail.ProductId;
                purchaseDto.ProductName = product.Name;

                purchaseDto.UnitPrice = thisPurchaseDetail.UnitPrice;
                purchaseDto.CloseAt = purchase.CloseAt;
                purchaseDtos.Add(purchaseDto);
            }

            return ZServiceResult<List<PurchaseDTO>>.Success(
                "Truyền dữ liệu hoàn tất",
                purchaseDtos
            );
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<PurchaseDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<List<PurchaseDTO>>> GetCustomerPurchases(string userId)
    {
        try
        {
            var customer = await _customerRepository.GetByProperty(c => c.UserId, userId);
            if (customer == null)
                return ZServiceResult<List<PurchaseDTO>>.Failure("Người dùng không hợp lệ", 400);
            var purchases = await _purchaseRepository.GetAllByProperty(
                p => p.CustomerId,
                customer.CustomerId
            );
            var purchaseDtos = new List<PurchaseDTO>();
            var purchaseDetails = await _purchaseDetailRepository.GetAllAsync();

            foreach (var purchase in purchases)
            {
                var purchaseDto = new PurchaseDTO();
                var thisPurchaseDetail = purchaseDetails.FirstOrDefault(p =>
                    p.PurchaseId == purchase.PurchaseId
                );
                if (thisPurchaseDetail == null)
                    return ZServiceResult<List<PurchaseDTO>>.Failure(
                        "Không tìm thấy chi tiết đơn hàng",
                        404
                    );
                var product = await _productRepository.GetByIdAsync(thisPurchaseDetail.ProductId);
                if (product == null || customer == null)
                    return ZServiceResult<List<PurchaseDTO>>.Failure(
                        "Không tìm thấy sản phẩm hoặc khách hàng",
                        404
                    );

                purchaseDto.PurchaseId = purchase.PurchaseId;

                purchaseDto.CustomerId = purchase.CustomerId;
                purchaseDto.CustomerFullname = customer.Fullname;

                purchaseDto.ProductId = thisPurchaseDetail.ProductId;
                purchaseDto.ProductName = product.Name;

                purchaseDto.UnitPrice = thisPurchaseDetail.UnitPrice;
                purchaseDto.CloseAt = purchase.CloseAt;
                purchaseDtos.Add(purchaseDto);
            }

            return ZServiceResult<List<PurchaseDTO>>.Success(
                "Truyền dữ liệu hoàn tất",
                purchaseDtos
            );
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<PurchaseDTO>>.Failure(ex.Message);
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
            var productDTOs = new List<ProductDTO>();
            foreach (var product in products)
            {
                var productDTO = _mapper.Map<ProductDTO>(product);
                var publisher = await _publisherRepository.GetByIdAsync(product.PublisherId);
                productDTO.PublisherName = publisher!.Name;
                productDTOs.Add(productDTO);
            }
            return ZServiceResult<List<ProductDTO>>.Success("Truyền dữ liệu hoàn tất", productDTOs);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<ProductDTO>>.Failure(ex.Message);
        }
    }
}
