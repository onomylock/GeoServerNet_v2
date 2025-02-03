using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.User;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadByEmailQuery;

public class UserReadByEmailQuery : IRequest<ResponseBase<UserReadResultDto>>
{
    [Required] public string Email { get; set; }
}