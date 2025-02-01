using FluentValidation;
using MasterServer.Application.Exceptions;
using MasterServer.Application.Models.Dto.User;
using MasterServer.Application.Services.Data;
using MasterServer.Infrastructure.Mappers;
using MediatR;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Exceptions;
using Shared.Common.Helpers;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserUpdateCommand;

public class UserUpdateHandler(
    IValidator<UserUpdateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserGroupEntityService userGroupEntityService
) : IRequestHandler<UserUpdateCommand, ResponseBase<UserReadResultDto>>
{
    public async Task<ResponseBase<UserReadResultDto>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);
            
            var isRoot = await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId, cancellationToken);

            if (!(isRoot || await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.ManageUsersUserGroupId, cancellationToken)))
                throw new InsufficientPermissionsException();

            var targetUser = await userEntityService.GetByIdAsync(request.UserId, true, cancellationToken) ??
                             throw new UserNotFoundException();

            var customPasswordHasher = new CustomPasswordHasher();

            targetUser.Alias = request.Alias;
            targetUser.FirstName = request.FirstName;
            targetUser.LastName = request.LastName;
            targetUser.Patronymic = request.Patronymic;

            if (!string.IsNullOrWhiteSpace(request.Password)) targetUser.PasswordHashed = customPasswordHasher.HashPassword(request.Password);

            targetUser.Active = request.Active;
            targetUser.Email = request.Email;

            await userEntityService.SaveAsync(targetUser, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<UserReadResultDto>
            {
                Data = await UserMapper.ToUserReadResultDto(targetUser, userGroupEntityService, isRoot,
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