using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Common.AuthenticationSchemeOptions;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Commands.AuthValidateJwtCommand;

public class AuthValidateJwtHandler(
    IValidator<AuthValidateJwtCommand> validator,
    IOptionsMonitor<JsonWebTokenAuthenticationSchemeOptions> jsonWebTokenAuthenticationSchemeOptionsMonitor
) : IRequestHandler<AuthValidateJwtCommand, ResponseBase<AuthValidateJwtResultBaseDto>>
{
    public async Task<ResponseBase<AuthValidateJwtResultBaseDto>> Handle(AuthValidateJwtCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(request, cancellationToken);

        var jsonWebTokenAuthenticationSchemeOptions =
            jsonWebTokenAuthenticationSchemeOptionsMonitor.Get(request.AuthenticationScheme);

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(request.Token, jsonWebTokenAuthenticationSchemeOptions.TokenValidationParameters,
            out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        return new ResponseBase<AuthValidateJwtResultBaseDto>
        {
            Data = new AuthValidateJwtResultBaseDto
            {
                Claims = jwtToken.Claims.Select(_ => new ClaimEntry
                {
                    Type = _.Type,
                    Value = _.Value,
                    ValueType = _.ValueType
                }).ToArray()
            }
        };
    }
}