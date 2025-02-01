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

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserDeleteCommand;

public class UserDeleteHandler(
    IValidator<UserDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserToUserGroupMappingEntityService userToUserGroupMappingEntityService
) : IRequestHandler<UserDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);

            if (!(await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId,
                      cancellationToken) ||
                  await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.ManageUsersUserGroupId,
                      cancellationToken)))
                throw new InsufficientPermissionsException();

            var targetUser = await userEntityService.GetByIdAsync(request.UserId, true, cancellationToken) ??
                             throw new UserNotFoundException();

            await userToUserGroupMappingEntityService.BulkDelete(
                query => query.Where(_ => _.EntityLeftId == targetUser.Id), cancellationToken);

            await userEntityService.DeleteAsync(targetUser, cancellationToken);

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