using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionDeleteCommand;

public class SolutionDeleteHandler(
    IValidator<SolutionDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    ISolutionEntityService solutionEntityService,
    IMinioService minioService
) : IRequestHandler<SolutionDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(SolutionDeleteCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetSolution = await solutionEntityService.GetByIdAsync(request.SolutionId, true, cancellationToken) ?? throw new SolutionNotFoundException();

            await minioService.DeleteAsync(targetSolution.FileName, targetSolution.BucketName, cancellationToken);
            
            await solutionEntityService.DeleteAsync(targetSolution, cancellationToken);
            
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