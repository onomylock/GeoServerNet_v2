using FluentValidation;
using MasterServer.Application.Models.Dto.SolutionType;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeCreateCommand;

public class SolutionTypeCreateHandler(
    IValidator<SolutionTypeCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionTypeCreateCommand, ResponseBase<SolutionTypeReadResultDto>>
{
    public async Task<ResponseBase<SolutionTypeReadResultDto>> Handle(SolutionTypeCreateCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.RollbackTransactionAsync(cancellationToken);

            var targetSolutionType = new Domain.Entities.SolutionType
            {
                Alias = request.Alias,
                ArgumentsMask = request.ArgumentsMask
            };

            await solutionTypeEntityService.SaveAsync(targetSolutionType, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<SolutionTypeReadResultDto>
            {
                Data = SolutionTypeMapper.ToSolutionTypeReadResultDto(targetSolutionType)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(cancellationToken);

            throw;
        }
    }
}