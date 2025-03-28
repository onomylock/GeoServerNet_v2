using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Cluster.Notification;
using MasterServer.Application.Services;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;

public class ClusterDeleteHandler(
    IValidator<ClusterDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
    IClusterNotificationService clusterNotificationService
) : IRequestHandler<ClusterDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(ClusterDeleteCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = await clusterEntityService.GetByIdAsync(request.ClusterId, true, cancellationToken) ??
                                throw new ClusterNotFoundException();

            await clusterEntityService.DeleteAsync(targetCluster, cancellationToken);

            await clusterToNodeMappingEntityService.BulkDelete(query =>
                query.Where(_ => _.EntityLeftId == targetCluster.Id), cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            await clusterNotificationService.SendClusterDeletedNotification(ClusterMapper.ToClusterDeletedNotificationDto(targetCluster), cancellationToken);
            
            return new ResponseBase<OkResult>
            {
                Data = new OkResult()
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}