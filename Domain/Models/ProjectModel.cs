namespace Domain.Models;

public class ProjectModel
{
    public Guid Id { get; set; }
    public string? ImageFileName { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EndDate { get; set; }

    // Använde AI här för att få reda på hur jag kunde visa hur många dagar kvar tills projektet är klar efter man har skapat en.
    public int DaysLeft => (EndDate.Date - DateTime.UtcNow.Date).Days;
}