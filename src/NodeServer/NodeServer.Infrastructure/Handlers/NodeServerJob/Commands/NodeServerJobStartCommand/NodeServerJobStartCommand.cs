using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Application.Models.Dto.NodeServerJob;
using Shared.Common.Models.DTO.Base;
using Shared.Domain.View;

namespace NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;

public class NodeServerJobStartCommand : IRequest<ResponseBase<NodeServerJobReadResultDto>>
{
    public List<KeyValueEntry> Metadata { get; set; }
    public Guid SolutionId { get; set; }
}