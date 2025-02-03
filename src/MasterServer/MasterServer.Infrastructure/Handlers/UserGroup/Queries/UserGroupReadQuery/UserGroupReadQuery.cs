using MasterServer.Application.Models.Dto.UserGroup;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.UserGroup.Queries.UserGroupReadQuery;

public class UserGroupReadQuery : UserGroupTargetBaseDto, IRequest<ResponseBase<UserGroupReadResultDto>>
{
}