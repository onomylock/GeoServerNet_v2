using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Auth;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Auth.Queries.AuthIsRevokedQuery;

public class AuthIsRevokedQuery : IRequest<ResponseBase<AuthIsRevokedResultBaseDto>>
{
    [Required] public Guid JsonWebTokenId { get; set; }
}