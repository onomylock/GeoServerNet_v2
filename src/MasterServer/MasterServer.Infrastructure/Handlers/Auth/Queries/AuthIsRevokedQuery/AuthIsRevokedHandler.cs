using FluentValidation;
using MasterServer.Application.Models.Dto.Auth;
using MasterServer.Application.Services.Data;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Queries.AuthIsRevokedQuery;

public class AuthIsRevokedHandler(
    IValidator<AuthIsRevokedQuery> validator,
    IJsonWebTokenRevokedEntityService jsonWebTokenRevokedEntityService
) : IRequestHandler<AuthIsRevokedQuery, ResponseBase<AuthIsRevokedResultBaseDto>>
{
    public async Task<ResponseBase<AuthIsRevokedResultBaseDto>> Handle(AuthIsRevokedQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return new ResponseBase<AuthIsRevokedResultBaseDto>
        {
            Data = new AuthIsRevokedResultBaseDto
            {
                IsRevoked = await jsonWebTokenRevokedEntityService.GetByJsonWebTokenId(request.JsonWebTokenId, true,
                    cancellationToken) is not null
            }
        };
    }
}