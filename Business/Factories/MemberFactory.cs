using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Factories;

public static class MemberFactory
{
    public static MemberEntity ToEntity(AddMemberForm? formData, string? newImageFileName = null)
    {
        if (formData is null) return null!;

        return new MemberEntity
        {
            ImageFileName = newImageFileName,
            FirstName = formData.FirstName,
            LastName = formData.LastName,
            Email = formData.Email,
            Phone = formData.Phone,
            Title = formData.Title,
            MemberRole = formData.MemberRole,
            Address = new MemberAddressEntity
            {
                StreetName = formData.StreetName,
                PostalCode = formData.PostalCode,
                CityName = formData.CityName
            }
        };
    }

    public static void UpdateEntity(MemberEntity entity, EditMemberForm formData, string? newImageFileName = null)
    {
        if (entity is null || formData is null) return;

        entity.FirstName = formData.FirstName;
        entity.LastName = formData.LastName;
        entity.Email = formData.Email;
        entity.Phone = formData.Phone;
        entity.Title = formData.Title;
        entity.MemberRole = formData.MemberRole;
        entity.ImageFileName = newImageFileName ?? formData.ImageFileName;

        entity.Address ??= new MemberAddressEntity();
        entity.Address.StreetName = formData.StreetName;
        entity.Address.PostalCode = formData.PostalCode;
        entity.Address.CityName = formData.CityName;
    }

    public static MemberModel ToModel(MemberEntity? entity)
    {
        if (entity is null) return null!;
        return new MemberModel
        {
            Id = entity.Id,
            ImageFileName = entity.ImageFileName,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            Title = entity.Title,
            MemberRole = entity.MemberRole
        };
    }
}
