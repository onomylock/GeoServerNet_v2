using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Solution;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Queries.SolutionReadCollectionSearchQuery;

public class SolutionReadCollectionSearchQuery : IRequest<ResponseBase<SolutionReadCollectionResultDto>>
{
    [MinLength(1)] [MaxLength(64)] public string Term { get; set; }

    [Required] public PageModel PageModel { get; set; }
}