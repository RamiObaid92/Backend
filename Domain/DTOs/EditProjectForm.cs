using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class EditProjectForm
{
    [Required]
    public Guid Id { get; set; }

    public string? ImageFileName { get; set; }
    public IFormFile? NewImageFile { get; set; }

    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public Guid ClientId { get; set; }

    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required]
    public string ProjectOwnerId { get; set; } = null!;

    public decimal Budget { get; set; }

    [Required]
    public int StatusId { get; set; }
}

