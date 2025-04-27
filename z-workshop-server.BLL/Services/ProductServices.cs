using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IProductService : IZBaseService<Product, ProductDTO> { }

public class ProductService(IProductRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Product, ProductDTO>(repository, mapper, worker, "Sản phẩm"),
        IProductService
{
    public async Task<ZServiceResult<string>> CreateProduct(
        ProductFormData productFormData,
        string opId
    )
    {
        try
        {
            var product = _mapper.Map<Product>(productFormData);
            product.ProductId = "product." + Guid.NewGuid().ToString("N");
            product.ApprovedBy = opId;

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
        ProductUpdateFormData productUpdateFormData,
        string opId
    )
    {
        try
        {
            var product = await _repository.GetByIdAsync(productUpdateFormData.ProductId);

            if (product == null)
                return ZServiceResult<string>.Failure("Không tìm thấy sản phẩm");

            _mapper.Map(productUpdateFormData, product);
            product.ApprovedBy = opId;

            _repository.Update(product);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"Đã cập nhật sản phẩm {product.Name}");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    // public async Task<ZServiceResult<string>> DeleteProduct(string productId){

    // }
}
