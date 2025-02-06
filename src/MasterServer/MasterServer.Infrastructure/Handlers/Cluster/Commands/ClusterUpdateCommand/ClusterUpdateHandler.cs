using FluentValidation;
using MasterServer.Application.Models.Dto.Cluster;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Commands.ClusterUpdateCommand;

public class ClusterUpdateHandler(
    IValidator<ClusterUpdateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IClusterEntityService clusterEntityService
) : IRequestHandler<ClusterUpdateCommand, ResponseBase<ClusterReadResultDto>>
{
    public async Task<ResponseBase<ClusterReadResultDto>> Handle(ClusterUpdateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetCluster = await clusterEntityService.GetByIdAsync(request.ClusterId, true, cancellationToken);
            
            targetCluster.LoadBalancingPolicy = request.LoadBalancingPolicy;
            
            await clusterEntityService.SaveAsync(targetCluster, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<ClusterReadResultDto>()
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