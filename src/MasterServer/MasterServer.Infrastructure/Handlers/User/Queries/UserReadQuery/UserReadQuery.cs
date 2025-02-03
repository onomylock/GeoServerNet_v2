using MasterServer.Application.Models.Dto.User;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadQuery;

public class UserReadQuery : UserTargetBaseDto, IRequest<ResponseBase<UserReadResultDto>>;