using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity ToEntity(SignUpForm? formData)
    {
        if (formData is null) return null!;

        return new UserEntity
        {
            UserName = formData.Email,
            FirstName = formData.FirstName,
            LastName = formData.LastName,
            Email = formData.Email,
            ImageFileName = "Default.png"
        };
    }

    public static UserModel ToModel(UserEntity? entity)
    {
        if (entity is null) return null!;

        return new UserModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageFileName = entity.ImageFileName
        };
    }
}
