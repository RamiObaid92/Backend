using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class SignInForm
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}

