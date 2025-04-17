using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Mappers;

public class ProjectFactory
{
    public static ProjectEntity ToEntity(AddProjectForm? formData, string? newImageFileName = null)
    {
        if (formData is null) return null!;
        return new ProjectEntity
        {
            ImageFileName = newImageFileName,
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            Budget = formData.Budget,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            ProjectOwnerId = formData.ProjectOwnerId,
            ClientId = formData.ClientId,
            StatusId = 1
        };
    }

    public static ProjectEntity ToEntity(EditProjectForm? formData, string? newImageFileName = null)
    {
        if (formData is null) return null!;
        return new ProjectEntity
        {
            Id = formData.Id,
            ImageFileName = newImageFileName,
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            Budget = formData.Budget,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            ProjectOwnerId = formData.ProjectOwnerId,
            ClientId = formData.ClientId,
            StatusId = formData.StatusId
        };
    }

    public static ProjectModel ToModel(ProjectEntity? entity)
    {
        if (entity is null) return null!;
        return new ProjectModel
        {
            Id = entity.Id,
            ImageFileName = entity.ImageFileName,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            EndDate = entity.EndDate,
        };
    }
}
