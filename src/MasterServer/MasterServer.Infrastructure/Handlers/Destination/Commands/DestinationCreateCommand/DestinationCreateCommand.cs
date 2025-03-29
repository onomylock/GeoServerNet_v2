using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Destination;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Node.Commands.NodeCreateCommand;

public class DestinationCreateCommand : IRequest<ResponseBase<DestinationReadResultDto>>
{
    [Required] private UriData Address { get; set; }
}