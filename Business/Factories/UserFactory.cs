using Data.Entities;
using Domain.DTOs;
using Domain.Models;
using System.Data;

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
            Email = formData.Email
        };
    }

    public static UserModel ToModel(UserEntity? entity, IList<string>? roles = null)
    {
        if (entity is null) return null!;

        return new UserModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageFileName = entity.ImageFileName,
            Roles = roles?.ToList() ?? new List<string>()
        };
    }
}
