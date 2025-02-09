using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.Solution;

public class SolutionTargetRequestDto
{
    [Required] public Guid SolutionId { get; set; }
}