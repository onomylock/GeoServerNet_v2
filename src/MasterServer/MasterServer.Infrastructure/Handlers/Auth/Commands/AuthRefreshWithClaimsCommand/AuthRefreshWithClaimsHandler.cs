using FluentValidation;
using MasterServer.Application.Models.Dto.Auth;
using MasterServer.Application.Models.Options;
using MasterServer.Application.Services.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Models.DTO.Base;
using Shared.Common.Models.Options;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthRefreshWithClaimsCommand;

public class AuthRefreshWithClaimsHandler(
    IDbContextTransactionAction dbContextTransactionAction,
    IValidator<AuthRefreshWithClaimsCommand> validator,
    IOptions<MasterServerOptions> masterServiceOptions,
    IUserAdvancedService userAdvancedService,
    IJsonWebTokenAdvancedService jsonWebTokenAdvancedService,
    IHttpContextAccessor httpContextAccessor,
    IOptions<JsonWebTokenOptions> jsonWebTokenOptions,
    IJsonWebTokenRevokedEntityService jsonWebTokenRevokedEntityService,
    IRefreshTokenEntityService refreshTokenEntityService
) : AuthRefreshHandlerBase(
    httpContextAccessor,
    jsonWebTokenOptions,
    masterServiceOptions,
    userAdvancedService,
    jsonWebTokenAdvancedService,
    jsonWebTokenRevokedEntityService,
    refreshTokenEntityService), IRequestHandler<AuthRefreshWithClaimsCommand, ResponseBase<AuthSignInResultBaseDto>>
{
    public async Task<ResponseBase<AuthSignInResultBaseDto>> Handle(AuthRefreshWithClaimsCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            var result = await RefreshBase(request, request.Claims, cancellationToken);

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