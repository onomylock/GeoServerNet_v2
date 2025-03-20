using Shared.Common.Models.DTO.Base;

namespace Shared.Common.Models.Dto;

public record ErrorModelResult
{
    public List<ErrorBase> Errors { get; set; }
}