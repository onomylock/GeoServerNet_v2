using FluentValidation;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;

public class ClusterCreateHandler(
    IValidator<ClusterCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
    INodeEntityService nodeEntityService,
    IClusterNotificationService notificationService
) : IRequestHandler<ClusterCreateCommand, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterCreateCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = new Domain.Entities.Cluster
            {
                LoadBalancingPolicy = request.LoadBalancingPolicy
            };

            var target = await clusterEntityService.AddAsync(targetCluster, cancellationToken);

            var targetNodes = await nodeEntityService.GetCollection(PageModel.Full,
                query => query.Where(x => request.NodeIds.Contains(x.Id)), true, cancellationToken);
            
            var targetClusterToNodeMappings =
                targetNodes.entities.Select(x => ClusterToNodeMappingMapper.ToClusterToNodeMapping(target.Id, x.Id));

            await clusterToNodeMappingEntityService.SaveAsync(targetClusterToNodeMappings, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            var notification = ClusterMapper.ToClusterCreatedNotificationDto(target, targetNodes.entities);

            await notificationService.SendClusterCreatedNotification(notification, cancellationToken);

            return new ResponseBase<ClusterReadResultDto>
            {
                Data = await ClusterMapper.ToClusterReadResultDto(targetCluster, clusterToNodeMappingEntityService,
                    cancellationToken)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}