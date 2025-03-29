using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Destination;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Node.Commands.NodeUpdateCommand;

public class DestinationUpdateCommand : DestinationTargetRequestDto, IRequest<ResponseBase<DestinationReadResultBase>>
{
    [Required] public UriData Address { get; set; }
}