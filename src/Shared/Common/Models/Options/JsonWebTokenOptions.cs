using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models.Options;

public class JsonWebTokenOptions
{
    public string Issuer { get; set; }
    public bool ValidateIssuer { get; set; }
    public string Audience { get; set; }
    public bool ValidateAudience { get; set; }
    [Required] [MinLength(1)] public string IssuerSigningKey { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    [Required] public int ExpirySeconds { get; set; }
}