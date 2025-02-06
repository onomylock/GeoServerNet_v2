using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterCreateCommand;

public class ClusterCreateHandler(
    IValidator<ClusterCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction, 
    IClusterEntityService clusterEntityService, 
    IUserEntityService userEntityService
) : IRequestHandler<ClusterCreateCommand, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetUser = await userEntityService.GetByIdAsync(request.UserId, true, cancellationToken) ??
                             throw new UserNotFoundException();
            
            var targetCluster = new Domain.Entities.Cluster
            {
                UserId = targetUser.Id,
                LoadBalancingPolicy = request.LoadBalancingPolicy
            };

            await clusterEntityService.SaveAsync(targetCluster, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

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