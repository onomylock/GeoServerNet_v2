using System.Diagnostics;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Application.Exceptions;
using NodeServer.Application.Models.Dto.NodeServerJob;
using NodeServer.Application.Services.Data;
using NodeServer.Infrastructure.Helpers;
using NodeServer.Infrastructure.Mappers;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Models.DTO.Base;
using Shared.Domain.View;

namespace NodeServer.Infrastructure.Handlers.NodeServerJob.Commands.NodeServerJobStartCommand;

public class NodeServerJobStartHandler(
    IValidator<NodeServerJobStartCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    INodeServerSolutionEntityService nodeServerSolutionEntityService,
    INodeServerJobEntityService jobEntityService,
    IHangfireService hangfireService
) : IRequestHandler<NodeServerJobStartCommand, ResponseBase<NodeServerJobReadResultDto>>
{
    public async Task<ResponseBase<NodeServerJobReadResultDto>> Handle(NodeServerJobStartCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetNodeServerSolution =
                await nodeServerSolutionEntityService.GetByMasterServerSolutionIdAsync(request.SolutionId, false,
                    cancellationToken) ?? throw new NodeServerSolutionNotFoundException();

            var workingDirectory = FileHelper.CreateTmpDirectory(targetNodeServerSolution.DirectoryResultsPath,
                targetNodeServerSolution.MasterServerSolutionId);
            
            var processStartInfo = ProcessHelper.ConfigureProcessStartInfo(request.Metadata, workingDirectory.ToString());
            
            var jobId = hangfireService.AddEnque(() => Process.Start(processStartInfo));

            var targetJob = new Domain.Entities.NodeServerJob
            {
                SolutionId = targetNodeServerSolution.Id,
                Metadata = request.Metadata.ToArray(),
                TmpResultPath = workingDirectory.ToString(),
                JobId = Guid.Parse(jobId)
            };

            await jobEntityService.SaveAsync(targetJob, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);
            
            return new ResponseBase<NodeServerJobReadResultDto>()
            {
                Data = NodeServerJobMapper.ToNodeServerJobReadResultDto(targetJob)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);
            
            throw;
        }
    }
}