using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ImageFileName { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Title { get; set; } = null!;
    public string MemberRole { get; set; } = null!;

    public MemberAddressEntity? Address { get; set; }
}
