using FluentValidation;
using MasterServer.Application.Models.Dto.Auth;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaAliasCommand;

public class AuthSignInViaAliasHandler(
    IDbContextTransactionAction dbContextTransactionAction,
    IValidator<AuthSignInViaAliasCommand> validator,
    IUserEntityService userEntityService,
    IHttpContextAccessor httpContextAccessor,
    IOptions<JsonWebTokenOptions> jsonWebTokenOptions,
    IOptions<MasterServerOptions> masterServiceOptions,
    IRefreshTokenEntityService refreshTokenEntityService
) : AuthSingInHandlerBase(
    httpContextAccessor,
    jsonWebTokenOptions,
    masterServiceOptions,
    refreshTokenEntityService), IRequestHandler<AuthSignInViaAliasCommand, ResponseBase<AuthSignInResultBaseDto>>
{
    public async Task<ResponseBase<AuthSignInResultBaseDto>> Handle(AuthSignInViaAliasCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var user = await userEntityService.GetByAliasAsync(request.Alias, false, cancellationToken);

            var result = await SignInBase(request, user, null, cancellationToken);

            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<AuthSignInResultBaseDto>
            {
                Data = result.result,
                Errors = result.errors
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);

            throw;
        }
    }
}