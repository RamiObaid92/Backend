namespace Domain.Models;

public class ClientModel
{
    public Guid Id { get; set; }
    public string? ImageFileName { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Phone { get; set; }
    public string Email { get; set; } = null!;
    public string? City { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}
