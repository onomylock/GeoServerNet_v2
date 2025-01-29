using System.ComponentModel.DataAnnotations;

namespace Shared.Common.Models.Options;

public class DbContextOptions
{
    [Required] [MinLength(1)] public string ConnectionString { get; set; }
}