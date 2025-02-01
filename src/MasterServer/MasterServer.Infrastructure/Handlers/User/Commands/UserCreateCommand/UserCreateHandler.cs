using FluentValidation;
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

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserCreateCommand;

public class UserCreateHandler(
    IValidator<UserCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    IUserAdvancedService userAdvancedService,
    IUserEntityService userEntityService,
    IUserGroupEntityService userGroupEntityService
) : IRequestHandler<UserCreateCommand, ResponseBase<UserReadResultDto>>
{
    public async Task<ResponseBase<UserReadResultDto>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var userId = userAdvancedService.GetUserIdFromHttpContext(true);

            var isRoot = await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.RootUserGroupId, cancellationToken);

            if (!(isRoot || await userAdvancedService.IsInUserGroupByUserGroupId(userId, Consts.ManageUsersUserGroupId, cancellationToken)))
                throw new InsufficientPermissionsException();

            var customPasswordHasher = new CustomPasswordHasher();

            var targetUser = new Domain.Entities.User
            {
                Alias = request.Alias,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                PasswordHashed = customPasswordHasher.HashPassword(request.Password),
                Active = request.Active,
                Email = request.Email
            };

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