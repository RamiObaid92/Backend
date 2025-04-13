using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ImageFileName { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Budget {  get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [ForeignKey(nameof(ProjectOwner))]
    public string ProjectOwnerId { get; set; } = null!;
    public UserEntity ProjectOwner { get; set; } = null!;

    [ForeignKey(nameof(Client))]
    public Guid ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;
}
