using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Commands.SolutionTypeDeleteCommand;

public class SolutionTypeDeleteHandler(
    IValidator<SolutionTypeDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    ISolutionTypeEntityService solutionTypeEntityService
) : IRequestHandler<SolutionTypeDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(SolutionTypeDeleteCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var targetSolutionType =
                await solutionTypeEntityService.GetByIdAsync(request.SolutionTypeId, true, cancellationToken) ??
                throw new SolutionTypeNotFoundException();

            await solutionTypeEntityService.DeleteAsync(targetSolutionType, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

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