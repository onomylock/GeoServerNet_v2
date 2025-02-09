using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Application.Exceptions;
using NodeServer.Application.Services;
using NodeServer.Application.Services.Data;
using Shared.Application.Data;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionDeleteCommand;

public class NodeServerSolutionDeleteHandler(
    IValidator<NodeServerSolutionDeleteCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    INodeServerSolutionEntityService nodeServerSolutionEntityService,
    IFileService fileService
    ) : IRequestHandler<NodeServerSolutionDeleteCommand, ResponseBase<OkResult>>
{
    public async Task<ResponseBase<OkResult>> Handle(NodeServerSolutionDeleteCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetNodeServerSolution = await nodeServerSolutionEntityService.GetByMasterServerSolutionIdAsync(request.MasterServerSolutionId, true,
                cancellationToken) ?? throw new NodeServerSolutionNotFoundException();
            
            await fileService.DeleteFolderAsync(targetNodeServerSolution.DirectoryPath, cancellationToken);
            
            await nodeServerSolutionEntityService.DeleteAsync(targetNodeServerSolution, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<OkResult>()
            {
                Data = new OkResult()
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);
            
            throw;
        }
    }
}