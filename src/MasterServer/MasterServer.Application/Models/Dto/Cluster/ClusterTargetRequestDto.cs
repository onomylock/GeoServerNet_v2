using System.ComponentModel.DataAnnotations;

namespace MasterServer.Application.Models.Dto.Cluster;

public class ClusterTargetRequestDto
{
    [Required] public string Alias { get; set; }
}