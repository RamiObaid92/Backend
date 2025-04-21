using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class AddMemberForm
{
    public IFormFile? ImageFile { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? StreetName { get; set; }

    public string? PostalCode { get; set; }

    public string? CityName { get; set; }
}

