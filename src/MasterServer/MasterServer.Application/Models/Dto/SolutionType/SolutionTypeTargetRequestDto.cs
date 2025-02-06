using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.SolutionType;

public class SolutionTypeTargetRequestDto
{
    [Required] public Guid SolutionTypeId { get; set; }
}