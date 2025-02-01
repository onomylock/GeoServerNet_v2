using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.User;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.UserCreateCommand;

public class UserCreateCommand : IRequest<ResponseBase<UserReadResultDto>>
{
    [Required] [MinLength(1)] public string Alias { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Patronymic { get; set; }

    [Required] [MinLength(8)] public string Password { get; set; }

    public bool Active { get; set; }

    [RegularExpression(RegexExpressions.Email)]
    public string Email { get; set; }

}