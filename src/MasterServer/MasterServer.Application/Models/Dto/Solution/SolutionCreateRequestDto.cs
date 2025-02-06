using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MasterServer.Application.Models.Dto.Solution;

public class SolutionCreateRequestDto
{
    [Required] public IFormFile File { get; set; }
    [Required] public long FileSize { get; set; }
    [Required] public string SolutionTypeAlias { get; set; }
}