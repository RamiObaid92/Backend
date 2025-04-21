namespace Domain.Models;

public class UserModel
{
    public string Id { get; set; } = null!;
    public string? ImageFileName { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public List<string> Roles { get; set; } = [];
}
