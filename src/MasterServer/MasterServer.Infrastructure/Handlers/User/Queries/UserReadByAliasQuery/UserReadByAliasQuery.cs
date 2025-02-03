using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.User;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadByAliasQuery;

public class UserReadByAliasQuery : IRequest<ResponseBase<UserReadResultDto>>
{
    [Required] public string Alias { get; set; }
}