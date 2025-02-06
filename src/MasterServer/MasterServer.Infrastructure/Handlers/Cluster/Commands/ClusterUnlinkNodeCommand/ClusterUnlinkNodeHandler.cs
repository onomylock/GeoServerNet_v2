using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUnlinkNodeCommand;

public class ClusterUnlinkNodeHandler(
    IValidator<ClusterUnlinkNodeCommand> validator,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService
) : IRequestHandler<ClusterUnlinkNodeCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(ClusterUnlinkNodeCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = await clusterEntityService.GetByIdAsync(request.ClusterId, true, cancellationToken) ??
                                throw new ClusterNotFoundException();

            var errors = new List<ErrorBase>();
            
            foreach (var nodeId in request.NodeIds)
                try
                {
                    var targetClusterToNodeMapping =
                        await clusterToNodeMappingEntityService.GetByEntityLeftIdEntityRightIdAsync(targetCluster.Id,
                            nodeId, true, cancellationToken) ??
                                     throw new NodeNodFoundException();

                    await clusterToNodeMappingEntityService.DeleteAsync(targetClusterToNodeMapping, cancellationToken);
                }
                catch (Exception)
                {
                    errors.Add(new ErrorBase
                    {
                        ErrorMessage = Localize.Keys.Warning.MappingAlreadyExists
                    });
                }
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<OkResult>()
            {
                Data = new OkResult(),
                Errors = errors
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);
            
            throw;
        }
    }
}