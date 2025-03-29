using FluentValidation;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Hubs;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUpdateCommand;

public class ClusterUpdateHandler(
    IValidator<ClusterUpdateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    IHubContext<ClusterHub, IClusterHubActions> clusterHub
) : IRequestHandler<ClusterUpdateCommand, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterUpdateCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = await clusterEntityService.GetByAliasAsync(request.Alias, true, cancellationToken);

            targetCluster.LoadBalancingPolicy = request.LoadBalancingPolicy;
            targetCluster.RouteAlias = request.RouteAlias;
            targetCluster.HealthCheckInterval = request.HealthCheckInterval;
            targetCluster.HealthCheckPath = request.HealthCheckPath;

            await clusterEntityService.SaveAsync(targetCluster, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            await clusterHub.Clients.All.SendClusterUpdated(
                ClusterMapper.ToClusterUpdatedNotificationDto(targetCluster));

            return new ResponseBase<ClusterReadResultDto>
            {
                Data = await ClusterMapper.ToClusterReadResultDto(targetCluster, null, cancellationToken)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}