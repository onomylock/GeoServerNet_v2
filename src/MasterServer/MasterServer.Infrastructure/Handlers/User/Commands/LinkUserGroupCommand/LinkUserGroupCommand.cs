using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.User.Commands.LinkUserGroupCommand;

public class LinkUserGroupCommand : UserTargetBaseDto, IRequest<ResponseBase<OkResult>>
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public Guid[] UserGroupIds { get; set; }
}