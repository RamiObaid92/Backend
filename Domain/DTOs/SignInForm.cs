using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class SignInForm
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}

