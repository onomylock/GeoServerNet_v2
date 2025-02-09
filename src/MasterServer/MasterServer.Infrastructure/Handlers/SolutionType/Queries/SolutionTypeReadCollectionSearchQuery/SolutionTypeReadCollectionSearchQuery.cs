using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.SolutionType;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.SolutionType.Queries.SolutionTypeReadCollectionSearchQuery;

public class SolutionTypeReadCollectionSearchQuery : IRequest<ResponseBase<SolutionTypeReadCollectionResultDto>>
{
    [MinLength(1)] [MaxLength(64)] public string Term { get; set; }

    [Required] public PageModel PageModel { get; set; }
}