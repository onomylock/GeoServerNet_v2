using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;

public class NodeServerJobStartCommand : IRequest<ResponseBase<OkResult>>
{
    public string Arguments { get; set; }
    public Guid SolutionId { get; set; }
}