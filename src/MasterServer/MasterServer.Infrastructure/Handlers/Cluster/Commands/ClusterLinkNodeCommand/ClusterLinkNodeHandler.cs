using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterLinkNodeCommand;

public class ClusterLinkNodeHandler(
    IValidator<ClusterLinkNodeCommand> validator,
    IClusterToNodeMappingEntityService clusterToNodeMappingEntityService,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    INodeEntityService nodeEntityService) : IRequestHandler<ClusterLinkNodeCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(ClusterLinkNodeCommand request, CancellationToken cancellationToken)
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
                    var targetNode = await nodeEntityService.GetByIdAsync(nodeId, true, cancellationToken) ??
                                     throw new NodeNodFoundException();

                    await clusterToNodeMappingEntityService.SaveAsync(new ClusterToNodeMapping
                    {
                        EntityLeftId = targetCluster.Id,
                        EntityRightId = targetNode.Id
                    }, cancellationToken);
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