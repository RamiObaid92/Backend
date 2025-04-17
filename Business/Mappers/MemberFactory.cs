using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Mappers;

public class MemberFactory
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

    public static MemberEntity ToEntity(EditMemberForm? formData, string? newImageFileName = null)
    {
        if (formData is null) return null!;
        return new MemberEntity
        {
            Id = formData.Id,
            ImageFileName = newImageFileName ?? formData.ImageFileName,
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
