using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionCreateCommand;

public class SolutionCreateHandler(
    IValidator<SolutionCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    ISolutionEntityService solutionEntityService,
    ISolutionTypeEntityService solutionTypeEntityService,
    IUserAdvancedService userAdvancedService,
    IMinioService minioService
) : IRequestHandler<SolutionCreateCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(SolutionCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);
            
            var isRoot =
                await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId, cancellationToken);

            if (!(isRoot ||
                  await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.ManageUsersUserGroupId,
                      cancellationToken)))
                throw new InsufficientPermissionsException();
            
            var targetSolutionType =
                await solutionTypeEntityService.GetByAliasAsync(request.SolutionTypeAlias, false, cancellationToken) ??
                throw new SolutionTypeNotFoundException();
            
            var targetSolution = new Domain.Entities.Solution
            {
                FileName = Guid.NewGuid() + new FileInfo(request.FileName).Extension,
                BucketName = S3BucketKey.SolutionBucket,
                SolutionTypeId = targetSolutionType.Id,
            };

            await solutionEntityService.SaveAsync(targetSolution, cancellationToken);
            
            await minioService.SaveAsync(request.FileStream, targetSolution.FileName, targetSolution.BucketName, request.FileSize, cancellationToken);
         
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