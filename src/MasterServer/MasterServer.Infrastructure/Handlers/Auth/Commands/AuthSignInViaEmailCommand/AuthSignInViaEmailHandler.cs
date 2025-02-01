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

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthSignInViaEmailCommand;

public class AuthSignInViaEmailHandler(
    IDbContextTransactionAction dbContextTransactionAction,
    IValidator<AuthSignInViaEmailCommand> validator,
    IUserEntityService userEntityService,
    IHttpContextAccessor httpContextAccessor,
    IOptions<JsonWebTokenOptions> jsonWebTokenOptions,
    IOptions<MasterServiceOptions> masterServiceOptions,
    IRefreshTokenEntityService refreshTokenEntityService
) : AuthSingInHandlerBase(
    httpContextAccessor,
    jsonWebTokenOptions,
    masterServiceOptions,
    refreshTokenEntityService), IRequestHandler<AuthSignInViaEmailCommand, ResponseBase<AuthSignInResultBaseDto>>
{
    public async Task<ResponseBase<AuthSignInResultBaseDto>> Handle(AuthSignInViaEmailCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var user = await userEntityService.GetByEmailAsync(request.Email, false, cancellationToken);

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