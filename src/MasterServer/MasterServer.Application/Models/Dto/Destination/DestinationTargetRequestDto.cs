using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.Destination;

public class DestinationTargetRequestDto
{
    [Required] public string Alias { get; set; }
}