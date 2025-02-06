using MasterServer.Application.Models.Dto.User;
using MasterServer.Application.Services.Data;
using MasterServer.Domain.Entities;
using Shared.Common.Models;

namespace MasterServer.Infrastructure.Mappers;

public static class UserMapper
{
    public static async Task<UserReadResultDto> ToUserReadResultDto(
        User user,
        IUserGroupEntityService userGroupEntityService,
        bool displaySensitiveData = false,
        CancellationToken cancellationToken = default
    )
    {
        return await ToUserReadOutDto(user, userGroupEntityService, displaySensitiveData, cancellationToken);
    }

    private static async Task<UserReadResultDto> ToUserReadOutDto(
        User user,
        IUserGroupEntityService userGroupEntityService,
        bool displaySensitiveData = false,
        CancellationToken cancellationToken = default
    )
    {
        return new UserReadResultDto
        {
            Alias = user.Alias,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            PasswordHashed = displaySensitiveData ? user.PasswordHashed : null,
            Active = user.Active,
            Email = user.Email,
            UserGroupIds = userGroupEntityService is not null
                ? (await userGroupEntityService.GetByUserIdAsync(user.Id, PageModel.Full, query => query, true,
                    cancellationToken)).entities.Select(_ => _.Id).ToArray()
                : null,
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static async Task<UserReadCollectionResultDto> ToUserReadCollectionResultDto(
        (int total, IReadOnlyCollection<User> entities) data,
        IUserGroupEntityService userGroupEntityService,
        bool displaySensitiveData = false,
        CancellationToken cancellationToken = default
    )
    {
        var items = new List<UserReadResultDto>(PageModel.Max.PageSize);

        foreach (var entity in data.entities)
            items.Add(await ToUserReadOutDto(entity, userGroupEntityService, displaySensitiveData, cancellationToken));

        return new UserReadCollectionResultDto
        {
            Total = data.total,
            Items = items
        };
    }
}