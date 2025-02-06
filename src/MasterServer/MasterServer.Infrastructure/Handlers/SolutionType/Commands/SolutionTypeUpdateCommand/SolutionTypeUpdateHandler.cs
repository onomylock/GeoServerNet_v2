using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.SolutionType;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeUpdateCommand;

public class SolutionTypeUpdateHandler(
    IValidator<SolutionTypeUpdateCommand> validator,
    ISolutionTypeEntityService solutionTypeEntityService,
    IDbContextTransactionAction dbContextTransactionAction
) : IRequestHandler<SolutionTypeUpdateCommand, ResponseBase<SolutionTypeReadResultDto>>
{
    public async Task<ResponseBase<SolutionTypeReadResultDto>> Handle(SolutionTypeUpdateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetSolutionType =
                await solutionTypeEntityService.GetByIdAsync(request.SolutionTypeId, true, cancellationToken) ??
                throw new SolutionTypeNotFoundException();
            
            targetSolutionType.Alias = request.Alias;
            targetSolutionType.ArgumentsMask = request.ArgumentsMask;
            
            await solutionTypeEntityService.SaveAsync(targetSolutionType, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<SolutionTypeReadResultDto>()
            {
                Data = SolutionTypeMapper.ToSolutionTypeReadResultDto(targetSolutionType)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);
            
            throw;
        }
    }
}