using Shared.Common.Models.DTO.Base;

namespace NodeServer.Application.Models.Dto.NodeServerJob;

public class NodeServerJobReadResultDto : EntityResponseBase
{
    public Guid JobId { get; set; }
}