using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Application.Exceptions;
using NodeServer.Application.Services.Data;
using Shared.Application.Services;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;

public class NodeServerJobStartHandler(
    IValidator<NodeServerJobStartCommand> validator,
    INodeServerSolutionEntityService nodeServerSolutionEntityService,
    IHangfireService hangfireService
) : IRequestHandler<NodeServerJobStartCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(NodeServerJobStartCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            var targetSolution =
                await nodeServerSolutionEntityService.GetByMasterServerSolutionIdAsync(request.SolutionId, false,
                    cancellationToken) ?? throw new NodeServerSolutionNotFoundException();
            
            //hangfireService.AddEnque()
            
            return new ResponseBase<OkResult>()
            {
                Data = new OkResult(),
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}