using System.ComponentModel.DataAnnotations;
using MediatR;
using NodeServer.Application.Models.Dto.NodeServerSolution;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionCreateCommand;

public class NodeServerSolutionCreateCommand : IRequest<ResponseBase<NodeServerSolutionReadResultDto>>
{
    [Required] public Guid MasterServerSolutionId { get; set; }
    [Required] public string FileName { get; set; }
    [Required] public string BucketName { get; set; }
    [Required] public string ArgumentsMask { get; set; }
}