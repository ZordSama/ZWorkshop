using AutoMapper;
using z_workshop_server.BLL.DTOs;
using z_workshop_server.DAL.Repositories;

namespace z_workshop_server.BLL.Services;

public interface IZBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<ZServiceResult<TDto>> GetByIdAsync(params object[] keys);
    Task<ZServiceResult<List<TDto>>> GetAllAsync();
    Task<ZServiceResult<TDto>> UpdateAsync(TDto dto, params object[] keys);
    Task<ZServiceResult<string>> DeleteAsync(params object[] keys);
}

public abstract class ZBaseService<TEntity, TDto> : IZBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;
    protected readonly IWorker _worker;
    protected readonly string _entityName;

    protected ZBaseService(
        IRepository<TEntity> repository,
        IMapper mapper,
        IWorker worker,
        string entityName
    )
    {
        _repository = repository;
        _mapper = mapper;
        _worker = worker;
        _entityName = entityName;
    }

    public virtual async Task<ZServiceResult<TDto>> GetByIdAsync(params object[] keys)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(keys);

            if (entity == null)
                return ZServiceResult<TDto>.Failure($"{_entityName} không tồn tại", 404);

            return ZServiceResult<TDto>.Success(
                "Truyền dữ liệu thành công",
                _mapper.Map<TDto>(entity)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<TDto>.Failure(ex.Message);
        }
    }

    public virtual async Task<ZServiceResult<List<TDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            return ZServiceResult<List<TDto>>.Success(
                "Truyền dữ liệu thành công",
                _mapper.Map<List<TDto>>(entities)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<List<TDto>>.Failure(ex.Message);
        }
    }

    public virtual async Task<ZServiceResult<TDto>> UpdateAsync(TDto dto, params object[] keys)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(keys);

            if (entity == null)
                return ZServiceResult<TDto>.Failure($"{_entityName} không tồn tại", 404);

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            await _worker.SaveChangesAsync();

            return ZServiceResult<TDto>.Success(
                $"Cập nhật {_entityName} {keys} thành công",
                _mapper.Map<TDto>(entity)
            );
        }
        catch (Exception ex)
        {
            return ZServiceResult<TDto>.Failure(ex.Message);
        }
    }

    public virtual async Task<ZServiceResult<string>> DeleteAsync(params object[] keys)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(keys);

            if (entity == null)
                return ZServiceResult<string>.Failure($"{_entityName} không tồn tại", 404);

            _repository.Delete(entity);
            await _worker.SaveChangesAsync();

            return ZServiceResult<string>.Success($"Đã xóa {_entityName} {keys}!");
        }
        catch (Exception ex)
        {
            return ZServiceResult<string>.Failure(ex.Message);
        }
    }
}
