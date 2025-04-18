using Business.Factories;
using Business.Handlers;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

// Methods in this class: GetAllStatusesAsync, UpdateCacheAsync.

public class StatusService(IStatusRepository statusRepository, ICacheHandler<IEnumerable<StatusModel>> cacheHandler)
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly ICacheHandler<IEnumerable<StatusModel>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "Statuses";

    public async Task<IEnumerable<StatusModel>?> GetAllStatusesAsync() 
        => _cacheHandler.GetFromCache(_cacheKey) ?? await UpdateCacheAsync();

    public async Task<IEnumerable<StatusModel>> UpdateCacheAsync()
    {
        var entities = await _statusRepository.GetAllAsync();
        var models = entities.Select(StatusFactory.ToModel).ToList();

        _cacheHandler.SetCache(_cacheKey, models);  
        return models;
    }
}
