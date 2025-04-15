using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class SignUpForm
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Password does not match")]
    public string PasswordConfirmation { get; set; } = null!;
}

