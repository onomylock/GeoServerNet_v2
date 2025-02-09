using System.ComponentModel.DataAnnotations;
using MasterServer.Application.Models.Dto.Cluster;
using MediatR;
using Shared.Common.Models;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Cluster.Queries.ClusterReadCollectionSearchQuery;

public class ClusterReadCollectionSearchQuery : IRequest<ResponseBase<ClusterReadCollectionResultDto>>
{
    [MinLength(1)] [MaxLength(64)] public string Term { get; set; }

    [Required] public PageModel PageModel { get; set; }
}