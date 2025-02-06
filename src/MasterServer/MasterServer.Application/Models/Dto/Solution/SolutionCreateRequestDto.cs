using MasterServer.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MasterServer.Application.Models.Dto.Solution;

public class SolutionCreateRequestDto
{
    public IFormFile File { get; set; }
    public long FileSize { get; set; }
    public string SolutionTypeAlias { get; set; }
}