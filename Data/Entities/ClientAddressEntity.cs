using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ClientAddressEntity
{
    [Key, ForeignKey(nameof(Client))]
    public Guid ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? BillingReference { get; set; }
}
