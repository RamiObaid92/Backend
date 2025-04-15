namespace Domain.Models;

public class MemberModel
{
    public Guid Id { get; set; }
    public string? ImageFileName { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string MemberRole { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";
}
