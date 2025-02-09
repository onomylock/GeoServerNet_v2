using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadQuery;

public class ClusterReadQuery : ClusterTargetRequestDto, IRequest<ResponseBase<ClusterReadResultDto>>
{
    
}