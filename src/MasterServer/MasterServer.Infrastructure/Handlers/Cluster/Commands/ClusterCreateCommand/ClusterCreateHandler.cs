using FluentValidation;
using MasterServer.Application.Models.Dto.Cluster;
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
    IClusterToDestinationMappingEntityService clusterToDestinationMappingEntityService,
    IDestinationEntityService destinationEntityService,
    IMediator mediator
) : IRequestHandler<ClusterCreateCommand, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterCreateCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetCluster = ClusterMapper.FromClusterCreateCommand(request);

            var target = await clusterEntityService.AddAsync(targetCluster, cancellationToken);
            
            var targetDestinations = await destinationEntityService.GetCollection(PageModel.Full,
                query => query.Where(x => request.Destinations.Contains(x.Alias)), true, cancellationToken);

            var targetClusterToDestinationMappings =
                targetDestinations.entities.Select(x => ClusterToDestinationMappingMapper.ToClusterToDestinationMapping(target.Id, x.Id));

            await clusterToDestinationMappingEntityService.SaveAsync(targetClusterToDestinationMappings, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            var notification = ClusterMapper.ToClusterCreatedNotification(target, targetDestinations.entities);

            await mediator.Publish(notification, cancellationToken);
            
            return new ResponseBase<ClusterReadResultDto>
            {
                Data = await ClusterMapper.ToClusterReadResultDto(targetCluster, clusterToDestinationMappingEntityService,
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