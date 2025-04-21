using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class AddClientForm
{
    public IFormFile? ImageFile { get; set; }

    [Required]
    public string ClientName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string? Phone {  get; set; }

    [Required]
    public string StreetName { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string CityName { get; set; } = null!;

    public string? BillingReference { get; set; }
}