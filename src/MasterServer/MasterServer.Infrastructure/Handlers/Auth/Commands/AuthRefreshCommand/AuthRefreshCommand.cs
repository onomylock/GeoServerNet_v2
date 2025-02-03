using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshCommand;

public class AuthRefreshCommand : AuthRefreshRequestBaseDto, IRequest<ResponseBase<AuthSignInResultBaseDto>>;