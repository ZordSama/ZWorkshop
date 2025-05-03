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
}

public class PublisherService(IPublisherRepository repository, IMapper mapper, IWorker worker)
    : ZBaseService<Publisher, PublisherDTO>(repository, mapper, worker, "Nhà phát hành"),
        IPublisherService
{
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
                var fileSaveResult = await FileHelper.SaveFile(
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
}
