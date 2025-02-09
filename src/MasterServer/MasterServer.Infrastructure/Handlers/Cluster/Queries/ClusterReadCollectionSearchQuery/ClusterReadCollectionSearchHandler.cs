using FluentValidation;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadCollectionSearchQuery;

public class ClusterReadCollectionSearchHandler(
    IValidator<ClusterReadCollectionSearchQuery> validator,
    IClusterEntityService clusterEntityService,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService
) : IRequestHandler<ClusterReadCollectionSearchQuery, ResponseBase<ClusterReadCollectionResultDto>>
{
    public async Task<ResponseBase<ClusterReadCollectionResultDto>> Handle(ClusterReadCollectionSearchQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var dataTerm = request.Term.ToLowerInvariant();

#pragma warning disable CA1862
        var targetClusters = await clusterEntityService.GetCollection(request.PageModel, query =>
        {
            return query
                .Where(_ => string.IsNullOrEmpty(request.Term)
                            || _.UserId.ToString().ToLower().Contains(dataTerm) || dataTerm.Contains(_.UserId.ToString().ToLower())
                            || _.LoadBalancingPolicy.ToLower().Contains(dataTerm) || dataTerm.Contains(_.LoadBalancingPolicy.ToLower()));
        }, true, cancellationToken);
#pragma warning restore CA1862

        return new ResponseBase<ClusterReadCollectionResultDto>
        {
            Data = await ClusterMapper.ToClusterReadCollectionResultDto(targetClusters, clusterToNodeMappingEntityService,
                cancellationToken)
        };
    }
}