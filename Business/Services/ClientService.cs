using Business.Factories;
using Business.Handlers;
using Data.Repositories;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IClientService
{
    Task<ClientModel?> CreateClientAsync(AddClientForm formData);
    Task<bool> DeleteClientAsync(Guid id);
    Task<IEnumerable<ClientModel>> GetAllClientsAsync();
    Task<ClientModel?> GetClientByIdAsync(Guid id);
    Task<ClientModel?> UpdateClientAsync(EditClientForm formData);
}

public class ClientService(IClientRepository clientRepository, ICacheHandler<IEnumerable<ClientModel>> cacheHandler, IFileHandler fileHandler) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ICacheHandler<IEnumerable<ClientModel>> _cacheHandler = cacheHandler;
    private readonly IFileHandler _fileHandler = fileHandler;
    private const string _cacheKey = "Clients";

    public async Task<ClientModel?> CreateClientAsync(AddClientForm formData)
    {
        var clientExists = await _clientRepository.ExistsAsync(x => x.Email == formData.Email);
        if (clientExists) return null;

        string? imageFileName = null;
        if (formData.ImageFile != null)
        {
            imageFileName = await _fileHandler.UploadFileAsync(formData.ImageFile);
        }

        var entity = ClientFactory.ToEntity(formData);
        entity.ImageFileName = imageFileName;

        await _clientRepository.AddAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        => _cacheHandler.GetFromCache(_cacheKey) ?? await UpdateCacheAsync();

    public async Task<ClientModel?> GetClientByIdAsync(Guid id)
    {
        var entity = _cacheHandler.GetFromCache(_cacheKey)?.FirstOrDefault(x => x.Id == id);
        if (entity is not null) return entity;

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == id);
    }

    public async Task<ClientModel?> UpdateClientAsync(EditClientForm formData)
    {
        var entity = await _clientRepository.GetAsync(x => x.Id == formData.Id);
        if (entity is null) return null;

        if (formData.NewImageFile != null)
        {
            var imageFileName = await _fileHandler.UploadFileAsync(formData.NewImageFile);
            formData.ImageFileName = imageFileName;
        }

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

    private async Task<IEnumerable<ClientModel>> UpdateCacheAsync()
    {
        var entities = await _clientRepository.GetAllAsync();
        var models = entities.Select(ClientFactory.ToModel).ToList();

        _cacheHandler.SetCache(_cacheKey, models);
        return models;
    }
}
