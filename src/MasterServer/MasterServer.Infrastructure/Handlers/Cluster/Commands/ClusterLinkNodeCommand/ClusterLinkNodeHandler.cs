using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Common.Enums;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterLinkNodeCommand;

public class ClusterLinkNodeHandler(
    IValidator<ClusterLinkNodeCommand> validator,
    IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService,
    IDestinationEntityService destinationEntityService) : IRequestHandler<ClusterLinkNodeCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(ClusterLinkNodeCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = await clusterEntityService.GetByAliasAsync(request.Alias, true, cancellationToken) ??
                                throw new ClusterNotFoundException();

            var errors = new List<ErrorBase>();

            foreach (var destinationAlias in request.DestinationsAlias)
                try
                {
                    var targetDestination = await destinationEntityService.GetByAliasAsync(destinationAlias, true, cancellationToken) ??
                                     throw new NodeNodFoundException();

                    await clusterToDestinationMappingEntityService.SaveAsync(new ClusterToDestinationMapping
                    {
                        EntityLeftId = targetCluster.Id,
                        EntityRightId = targetDestination.Id
                    }, cancellationToken);
                }
                catch (Exception)
                {
                    errors.Add(new ErrorModelResultEntry(ErrorType.Generic,
                        Localize.Keys.Warning.MappingAlreadyExists));
                }

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);


            return new ResponseBase<OkResult>
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