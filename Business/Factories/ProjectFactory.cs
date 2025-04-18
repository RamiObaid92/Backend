using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Factories;

public static class ProjectFactory
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

    // Tog hjälp av AI med hur man optimerar UpdateEntity metoden. Så att den inte replacear hela objektet utan bara uppdaterar de fälten som har ändrats.
    public static void UpdateEntity(ProjectEntity entity, EditProjectForm formData, string? newImageFileName = null)
    {
        if (entity is null || formData is null) return;

        entity.ProjectName = formData.ProjectName;
        entity.Description = formData.Description;
        entity.Budget = formData.Budget;
        entity.StartDate = formData.StartDate;
        entity.EndDate = formData.EndDate;
        entity.ProjectOwnerId = formData.ProjectOwnerId;
        entity.ClientId = formData.ClientId;
        entity.StatusId = formData.StatusId;
        entity.ImageFileName = newImageFileName ?? formData.ImageFileName;
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
