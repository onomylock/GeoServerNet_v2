using Shared.Common.Models.DTO.Base;

namespace MasterServer.Application.Models.Dto.Solution;

public class SolutionReadResultDto : EntityResponseBase
{
    public string FileName { get; set; }
    public string BucketName { get; set; }
    public string SolutionTypeAlias { get; set; }
}