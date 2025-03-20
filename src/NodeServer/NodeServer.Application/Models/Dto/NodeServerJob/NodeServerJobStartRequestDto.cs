using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.Domain.View;

namespace NodeServer.Application.Models.Dto.NodeServerJob;

public class NodeServerJobStartRequestDto
{
    [Required] public IFormFile File { get; set; }
    [Required] public long FileSize { get; set; }
    [Required] public List<KeyValueEntry> Metadata { get; set; }
    [Required] public Guid SolutionId { get; set; }
}