using System.ComponentModel.DataAnnotations;

namespace NodeServer.Application.Models.Dto.NodeServerSolution;

public class NodeServerSolutionTargetRequestDto
{
    [Required] public Guid MasterServerSolutionId { get; set; }
}