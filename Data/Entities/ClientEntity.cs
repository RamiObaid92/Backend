﻿namespace Data.Entities;

public class ClientEntity
{
    public Guid Id { get; set; }
    public string? ImageFileName { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Phone {  get; set; }
    public string Email { get; set; } = null!;

    public ClientAddressEntity? ClientAddress { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
