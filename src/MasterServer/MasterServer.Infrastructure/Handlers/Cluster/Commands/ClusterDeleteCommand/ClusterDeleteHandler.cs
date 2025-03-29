using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Hubs;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterDeleteCommand;

public class ClusterDeleteHandler(
    IValidator<ClusterDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
    IHubContext<ClusterHub, IClusterHubActions> clusterHub
) : IRequestHandler<ClusterDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(ClusterDeleteCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = await clusterEntityService.GetByAliasAsync(request.Alias, true, cancellationToken) ??
                                throw new ClusterNotFoundException();

            await clusterEntityService.DeleteAsync(targetCluster, cancellationToken);

            await clusterToDestinationMappingEntityService.BulkDelete(query =>
                query.Where(_ => _.EntityLeftId == targetCluster.Id), cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            await clusterHub.Clients.All.SenClusterDeleted(
                ClusterMapper.ToClusterDeletedNotificationDto(targetCluster));

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