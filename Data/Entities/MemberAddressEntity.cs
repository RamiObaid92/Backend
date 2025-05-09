﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MemberAddressEntity
{
    [Key, ForeignKey(nameof(Member))]
    public Guid MemberId { get; set; }
    public MemberEntity Member { get; set; } = null!;
    public string? StreetName { get; set; }
    public string? PostalCode { get; set; }
    public string? CityName { get; set; }
}