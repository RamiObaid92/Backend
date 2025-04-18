using Business.Handlers;
using Business.Mappers;
using Data.Repositories;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, ICacheHandler<IEnumerable<ClientModel>> cacheHandler)
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ICacheHandler<IEnumerable<ClientModel>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "Clients";

    public async Task<ClientModel?> CreateClientAsync(AddClientForm formData)
    {
        var clientExists = await _clientRepository.ExistsAsync(x => x.Email == formData.Email);
        if (clientExists) return null;

        var entity = ClientFactory.ToEntity(formData);
        await _clientRepository.AddAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<IEnumerable<ClientModel>> GetAllClientsAsync() 
        => _cacheHandler.GetFromCache(_cacheKey) ?? await UpdateCacheAsync();

    public async Task<ClientModel?> GetClientByIdAsync(Guid id)
    {
        var model = _cacheHandler.GetFromCache(_cacheKey)?.FirstOrDefault(x => x.Id == id);
        if (model is not null) return model;

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == id);
    }

    public async Task<ClientModel?> UpdateClientAsync(EditClientForm formData)
    {
        var entity = await _clientRepository.GetAsync(x => x.Id == formData.Id);
        if (entity is null) return null;

        ClientFactory.UpdateEntity(entity, formData);
        await _clientRepository.UpdateAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<bool> DeleteClientAsync(Guid id)
    {
        var entity = await _clientRepository.DeleteAsync(x => x.Id == id);
        if (entity) await UpdateCacheAsync();
        return entity;
    }

    public async Task<IEnumerable<ClientModel>> UpdateCacheAsync()
    {
        var entities = await _clientRepository.GetAllAsync();
        var models = entities.Select(ClientFactory.ToModel).ToList();

        _cacheHandler.SetCache(_cacheKey, models);
        return models;
    }
}
