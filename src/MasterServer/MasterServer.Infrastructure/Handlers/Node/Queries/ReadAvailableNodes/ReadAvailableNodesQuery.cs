using MasterServer.Application.Models.Dto.Node;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Node.Queries.ReadAvailableNodes;

public class ReadAvailableNodesQuery : IRequest<ResponseBase<ReadNodeResponseBase>>
{
    
}