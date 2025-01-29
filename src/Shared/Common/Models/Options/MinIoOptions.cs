using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models.Options;

public class MinioOptions
{
    [Required] [MinLength(1)] public string Endpoint { get; set; }
    [Required] [MinLength(1)] public string AccessKey { get; set; }
    [Required] [MinLength(1)] public string SecretKey { get; set; }
    public bool WithSsl { get; set; }
}