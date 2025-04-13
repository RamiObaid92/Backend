﻿using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ClientEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ImageFileName { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Phone {  get; set; }
    public string Email { get; set; } = null!;

    public ClientAddressEntity? ClientAddress { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
