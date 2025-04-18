using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class StatusFactory
{
    public static StatusModel ToModel(StatusEntity statusEntity)
    {
        if (statusEntity == null) return null!;

        return new StatusModel
        {
            Id = statusEntity.Id,
            StatusName = statusEntity.StatusName
        };
    }
}
