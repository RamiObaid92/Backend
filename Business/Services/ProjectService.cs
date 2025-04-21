using Business.Factories;
using Business.Handlers;
using Data.Repositories;
using Domain.DTOs;
using Domain.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectModel?> CreateProjectAsync(AddProjectForm formData);
    Task<bool> DeleteProjectAsync(Guid id);
    Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
    Task<ProjectModel?> GetProjectByIdAsync(Guid id);
    Task<ProjectModel?> UpdateProjectAsync(EditProjectForm formData);
}

public class ProjectService(IProjectRepository projectRepository, ICacheHandler<IEnumerable<ProjectModel>> cacheHandler, IFileHandler fileHandler) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICacheHandler<IEnumerable<ProjectModel>> _cacheHandler = cacheHandler;
    private readonly IFileHandler _fileHandler = fileHandler;
    private const string _cacheKey = "Projects";

    public async Task<ProjectModel?> CreateProjectAsync(AddProjectForm formData)
    {
        if (await _projectRepository.ExistsAsync(x => x.ProjectName == formData.ProjectName))
            return null;

        string? imageFileName = null;
        if (formData.ImageFile != null)
        {
            imageFileName = await _fileHandler.UploadFileAsync(formData.ImageFile);
        }

        var entity = ProjectFactory.ToEntity(formData);
        entity.ImageFileName = imageFileName;
        await _projectRepository.AddAsync(entity);  

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
        => _cacheHandler.GetFromCache(_cacheKey) ?? await UpdateCacheAsync();

    public async Task<ProjectModel?> GetProjectByIdAsync(Guid id)
    {
        var entity = _cacheHandler.GetFromCache(_cacheKey)?.FirstOrDefault(x => x.Id == id);
        if (entity is not null) return entity;

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == id);
    }

    public async Task<ProjectModel?> UpdateProjectAsync(EditProjectForm formData)
    {
        var entity = await _projectRepository.GetAsync(x => x.Id == formData.Id);
        if (entity is null) return null;

        if (formData.NewImageFile != null)
        {
            var imageFileName = await _fileHandler.UploadFileAsync(formData.NewImageFile);
            formData.ImageFileName = imageFileName;
        }

        ProjectFactory.UpdateEntity(entity, formData);
        await _projectRepository.UpdateAsync(entity);

        var models = await UpdateCacheAsync();
        return models.FirstOrDefault(x => x.Id == entity.Id);
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        var entity = await _projectRepository.DeleteAsync(x => x.Id == id);
        if (entity) await UpdateCacheAsync();
        return entity;
    }

    private async Task<IEnumerable<ProjectModel>> UpdateCacheAsync()
    {
        var entities = await _projectRepository.GetAllAsync();
        var models = entities.Select(ProjectFactory.ToModel).ToList();

        _cacheHandler.SetCache(_cacheKey, models);
        return models;
    }
}
