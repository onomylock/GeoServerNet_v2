using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadQuery;

public class ClusterReadHandler(
    IValidator<ClusterReadQuery> validator,
    IClusterEntityService clusterEntityService,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService
) : IRequestHandler<ClusterReadQuery, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterReadQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var targetCluster = await clusterEntityService.GetByIdAsync(request.ClusterId, true, cancellationToken) ??
                            throw new ClusterNotFoundException();

        return new ResponseBase<ClusterReadResultDto>
        {
            Data = await ClusterMapper.ToClusterReadResultDto(targetCluster, clusterToNodeMappingEntityService, cancellationToken)
        };
    }
}