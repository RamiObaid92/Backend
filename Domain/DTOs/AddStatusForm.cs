using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class AddStatusForm
{
    [Required]
    public string StatusName { get; set; } = null!;
}
