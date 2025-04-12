namespace Data.Entities;

public class StatusEntity
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
