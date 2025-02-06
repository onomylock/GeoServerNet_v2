using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.SolutionType;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeUpdateCommand;

public class SolutionTypeUpdateCommand : SolutionTypeTargetRequestDto, IRequest<ResponseBase<SolutionTypeReadResultDto>>
{
    [Required] [MinLength(1)] public string Alias { get; set; }
    [MinLength(1)] [MaxLength(64)] public string ArgumentsMask { get; set; }
}