using Data.Entities;
using Domain.DTOs;
using Domain.Models;

namespace Business.Mappers;

public static class ClientFactory
{
    public static ClientEntity ToEntity(AddClientForm? formData, string? newImageFileName = null)
    {
        if (formData is null) return null!;

        return new ClientEntity
        {
            ClientName = formData.ClientName,
            Phone = formData.Phone,
            Email = formData.Email,
            ImageFileName = newImageFileName,

            Address = new ClientAddressEntity
            {
                StreetName = formData.StreetName,
                PostalCode = formData.PostalCode,
                CityName = formData.CityName,
                BillingReference = formData.BillingReference
            }
        };
    }

    public static void UpdateEntity(ClientEntity entity, EditClientForm formData, string? newImageFileName = null)
    {
        if (entity is null || formData is null) return;

        entity.ClientName = formData.ClientName;
        entity.Phone = formData.Phone;
        entity.Email = formData.Email;
        entity.ImageFileName = newImageFileName ?? formData.ImageFileName;

        entity.Address ??= new ClientAddressEntity();
        entity.Address.StreetName = formData.StreetName;
        entity.Address.PostalCode = formData.PostalCode;
        entity.Address.CityName = formData.CityName;
        entity.Address.BillingReference = formData.BillingReference;
    }

    public static ClientModel ToModel(ClientEntity? entity)
    {
        if (entity is null) return null!;
        return new ClientModel
        {
            Id = entity.Id,
            ImageFileName = entity.ImageFileName,
            ClientName = entity.ClientName,
            Phone = entity.Phone,
            Email = entity.Email,
            Created = entity.Created,
            IsActive = entity.IsActive
        };
    }
}
