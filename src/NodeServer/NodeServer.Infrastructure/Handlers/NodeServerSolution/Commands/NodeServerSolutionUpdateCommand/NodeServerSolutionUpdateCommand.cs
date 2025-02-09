using System.ComponentModel.DataAnnotations;
using MediatR;
using NodeServer.Application.Models.Dto.NodeServerSolution;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionUpdateCommand;

public class NodeServerSolutionUpdateCommand : NodeServerSolutionTargetRequestDto, IRequest<ResponseBase<NodeServerSolutionReadResultDto>>
{
    [Required] public string FileName { get; set; }
    [Required] public string BucketName { get; set; }
    [Required] public string ArgumentsMask { get; set; }
}