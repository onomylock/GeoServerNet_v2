using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.User;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Queries.UserReadCollectionSearchQuery;

public class UserReadCollectionSearchQuery : IRequest<ResponseBase<UserReadCollectionResultDto>>
{
    [MinLength(1)] [MaxLength(64)] public string Term { get; set; }

    [Required] public PageModel PageModel { get; set; }

}