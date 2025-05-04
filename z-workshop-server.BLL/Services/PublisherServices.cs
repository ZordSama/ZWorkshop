using System.Threading.Tasks;
using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.BLL.Helpers;
using z_workshop_server.DAL.Models;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IPublisherService : IZBaseService<Publisher, PublisherDTO>
{
    Task<ZServiceResult<string>> CreatePublishertAsync(PublisherFormData publisherFormData);
    Task<ZServiceResult<string>> UpdatePublisherAsync(
        string publisherId,
        PublisherUpdateFormData publisherUpdateFormData
    );
    Task<ZServiceResult<string>> DeletePublisherAsync(string publisherId);
}

public class PublisherService(
    IPublisherRepository repository,
    IMapper mapper,
    IWorker worker,
    IProductRepository productRepository
)
    : ZBaseService<Publisher, PublisherDTO>(repository, mapper, worker, "Nhà phát hành"),
        IPublisherService
{
    protected readonly IProductRepository _productRepository = productRepository;

    public async Task<ZServiceResult<string>> CreatePublishertAsync(
        PublisherFormData publisherFormData
    )
    {
        try
        {
            var publisher = _mapper.Map<Publisher>(publisherFormData);
            publisher.PublisherId = $"publisher.{Guid.NewGuid():N}";

            var avtUrl = "";
            if (publisherFormData.FileAvt != null && publisherFormData.FileAvt.Length > 0)
            {
                var fileSaveResult = await FileHelper.SaveFileAsync(
                    publisherFormData.FileAvt,
                    "publisher_thumbnails",
                    publisher.PublisherId
                );
                if (!fileSaveResult.IsSuccess)
                    return fileSaveResult;
                avtUrl = fileSaveResult.Data;
            }
            publisher.Avt = avtUrl;
            await _repository.AddAsync(publisher);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"{publisher.Name} đã được thêm thành công");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> UpdatePublisherAsync(
        string publisherId,
        PublisherUpdateFormData publisherUpdateFormData
    )
    {
        if (publisherId == null || publisherId != publisherUpdateFormData.PublisherId)
            return ZServiceResult<string>.Failure("PublisherId không hợp lệ", 400);

        try
        {
            var publisher = await _repository.GetByIdAsync(publisherId);
            if (publisher == null)
                return ZServiceResult<string>.Failure("Không tìm thấy nhà phát hành", 404);

            var avtUrl = "";
            if (
                publisherUpdateFormData.FileAvt != null
                && publisherUpdateFormData.FileAvt.Length > 0
            )
            {
                var fileSaveResult = await FileHelper.SaveFileAsync(
                    publisherUpdateFormData.FileAvt,
                    "publisher_thumbnails",
                    publisher.PublisherId
                );
                if (!fileSaveResult.IsSuccess)
                    return fileSaveResult;
                avtUrl = fileSaveResult.Data;
            }
            _mapper.Map(publisherUpdateFormData, publisher);
            publisher.Avt = avtUrl;
            _repository.Update(publisher);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"{publisher.Name} đã được cập nhật thành công");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }

    public async Task<ZServiceResult<string>> DeletePublisherAsync(string publisherId)
    {
        try
        {
            var thisProducts = await _productRepository.GetAllByProperty(
                p => p.PublisherId,
                publisherId
            );
            if (thisProducts.Count > 0)
                return ZServiceResult<string>.Failure(
                    "Hãy xóa toàn bộ sản phẩm của nhà phát hành này!",
                    400
                );
            var publisher = await _repository.GetByIdAsync(publisherId);
            if (publisher == null)
                return ZServiceResult<string>.Failure("Không tìm thấy nhà phát hành", 404);

            if (publisher.Avt != null)
            {
                var fileDeleteResult = await FileHelper.RemoveFileAsync(publisher.Avt);
                if (!fileDeleteResult.IsSuccess)
                    return fileDeleteResult;
            }

            _repository.Delete(publisher);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"{publisher.Name} đã được xóa thành công");
        }
        catch (System.Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
