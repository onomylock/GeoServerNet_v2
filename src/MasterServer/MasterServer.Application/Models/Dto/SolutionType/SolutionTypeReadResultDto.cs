using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.SolutionType;

public class SolutionTypeReadResultDto : EntityResponseBase
{
    public string Alias { get; set; }
    public string ArgumentsMask { get; set; }
}