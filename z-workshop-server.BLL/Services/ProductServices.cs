using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Helpers;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IProductService : IZBaseService<Product, ProductDTO>
{
    Task<ZServiceResult<string>> CreateProduct(ProductFormData productFormData, string opId);
    Task<ZServiceResult<List<ProductDTO>>> GetAllWithPublisherNameAsync();
    Task<ZServiceResult<string>> UpdateProduct(
        string productId,
        ProductUpdateFormData productUpdateFormData,
        string opId
    );
    Task<ZServiceResult<string>> DeleteProductAsync(string productId);
}

public class ProductService(
    IProductRepository repository,
    IMapper mapper,
    IWorker worker,
    IEmployeeRepository employeeRepository
) : ZBaseService<Product, ProductDTO>(repository, mapper, worker, "Sản phẩm"), IProductService
{
    protected readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<ZServiceResult<List<ProductDTO>>> GetAllWithPublisherNameAsync()
    {
        try
        {
            var products = await _repository.GetAllWithIncludesAsync(p => p.Publisher);
            List<ProductDTO> productDTOs = [];

            foreach (var product in products)
            {
                var temp = _mapper.Map<ProductDTO>(product);
                temp.PublisherName = product.Publisher.Name;
                productDTOs.Add(temp);
            }

            return ZServiceResult<List<ProductDTO>>.Success(
                "Lấy danh sách sản phẩm thành công",
                productDTOs
            );
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<List<ProductDTO>>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> CreateProduct(
        ProductFormData productFormData,
        string opId
    )
    {
        try
        {
            var employee = await _employeeRepository.GetByProperty(e => e.UserId, opId);
            if (employee == null)
                return ZServiceResult<string>.Failure("Người dùng không có thẩm quyền", 401);

            var product = _mapper.Map<Product>(productFormData);

            product.ProductId = "product." + Guid.NewGuid().ToString("N");
            var fileUrl = "";
            if (productFormData.Thumbnail != null)
            {
                var fileSaveResult = await FileHelper.SaveFileAsync(
                    productFormData.Thumbnail,
                    "product_thumbnails",
                    product.ProductId
                );
                if (!fileSaveResult.IsSuccess)
                    return fileSaveResult;

                fileUrl = fileSaveResult.Data;
            }

            product.Desc = Newtonsoft.Json.JsonConvert.SerializeObject(
                new ProductDescJsonDTO { Description = product.Desc, Thumbnail = fileUrl }
            );

            product.ApprovedBy = employee.EmployeeId;

            await _repository.AddAsync(product);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"Đã thêm sản phẩm {product.Name}", default, 201);
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> UpdateProduct(
        string productId,
        ProductUpdateFormData productUpdateFormData,
        string opId
    )
    {
        if (productId != productUpdateFormData.ProductId)
            return ZServiceResult<string>.Failure("ID sản phẩm không khớp", 400);
        try
        {
            var employee = await _employeeRepository.GetByProperty(e => e.UserId, opId);
            if (employee == null)
                return ZServiceResult<string>.Failure("Người dùng không có thẩm quyền", 401);

            var product = await _repository.GetByIdAsync(productUpdateFormData.ProductId);

            if (product == null)
                return ZServiceResult<string>.Failure("Không tìm thấy sản phẩm");

            var productDesc = new ProductDescJsonDTO { Description = null, Thumbnail = null };
            if (product.Desc != null)
            {
                productDesc = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDescJsonDTO>(
                    product.Desc
                );
            }

            if (productUpdateFormData.Thumbnail != null)
            {
                var fileSaveResult = await FileHelper.SaveFileAsync(
                    productUpdateFormData.Thumbnail,
                    "product_thumbnails",
                    product.ProductId
                );
                if (!fileSaveResult.IsSuccess)
                    return fileSaveResult;

                productDesc!.Thumbnail = fileSaveResult.Data;
            }
            if (productUpdateFormData.Desc != null)
                productDesc!.Description = productUpdateFormData.Desc;

            _mapper.Map(productUpdateFormData, product);
            product.Desc = Newtonsoft.Json.JsonConvert.SerializeObject(productDesc);
            product.ApprovedBy = employee.EmployeeId;

            _repository.Update(product);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"Đã cập nhật sản phẩm {product.Name}");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> DeleteProductAsync(string productId)
    {
        try
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
                return ZServiceResult<string>.Failure("Không tìm thấy sản phẩm", 404);
            if (product.Desc != null)
            {
                var productDesc = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDescJsonDTO>(
                    product.Desc
                );

                if (productDesc != null && productDesc.Thumbnail != null)
                {
                    var removeResult = await FileHelper.RemoveFileAsync(productDesc.Thumbnail);
                    if (!removeResult.IsSuccess)
                        return removeResult;
                }
            }
            _repository.Delete(product);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success("Đã xóa sản phẩm");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
