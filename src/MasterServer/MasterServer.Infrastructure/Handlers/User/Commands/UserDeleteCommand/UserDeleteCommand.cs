using MasterServer.Application.Models.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserDeleteCommand;

public class UserDeleteCommand : UserTargetBaseDto, IRequest<ResponseBase<OkResult>>;