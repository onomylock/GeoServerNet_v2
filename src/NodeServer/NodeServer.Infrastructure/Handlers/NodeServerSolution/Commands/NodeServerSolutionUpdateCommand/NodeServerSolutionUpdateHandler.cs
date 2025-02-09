using FluentValidation;
using MediatR;
using NodeServer.Application.Exceptions;
using NodeServer.Application.Models.Dto.NodeServerSolution;
using NodeServer.Application.Services;
using NodeServer.Application.Services.Data;
using NodeServer.Infrastructure.Helpers;
using NodeServer.Infrastructure.Mappers;
using Shared.Application.Data;
using Shared.Application.Services;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionUpdateCommand;

public class NodeServerSolutionUpdateHandler(
    IValidator<NodeServerSolutionUpdateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    INodeServerSolutionEntityService nodeServerSolutionEntityService,
    HttpClient httpClient,
    IMinioService minioService,
    IFileService fileService
) : IRequestHandler<NodeServerSolutionUpdateCommand, ResponseBase<NodeServerSolutionReadResultDto>>
{
    public async Task<ResponseBase<NodeServerSolutionReadResultDto>> Handle(NodeServerSolutionUpdateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            var errors = new List<ErrorBase>(); 
            
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);
            
            var targetNodeServerSolution =
                await nodeServerSolutionEntityService.GetByMasterServerSolutionIdAsync(request.MasterServerSolutionId,
                    true, cancellationToken) ?? throw new NodeServerSolutionNotFoundException();
            
            await fileService.DeleteFolderAsync(targetNodeServerSolution.DirectoryPath, cancellationToken);
            
            var uri = await minioService.GetFileUrl(request.FileName, request.BucketName, cancellationToken);
            
            var requestStream = await httpClient.GetStreamAsync(uri, cancellationToken);

            var solutionPath = await fileService.ExtractArchiveAsync(requestStream, FileHelper.BuildsPath, cancellationToken);
            
            var workingDirectory = FileHelper.CreateSolutionResultsPath(solutionPath);

            var fileExePath = Path.Combine(solutionPath, request.FileName);
            
            if (!File.Exists(Path.Combine(solutionPath, request.FileName)))
                throw new FileNotFoundException();

            targetNodeServerSolution.DirectoryPath = solutionPath;
            targetNodeServerSolution.FileExePath = fileExePath;
            targetNodeServerSolution.DirectoryResultsPath = workingDirectory.ToString();
            targetNodeServerSolution.ArgumentsMask = request.ArgumentsMask;
            
            await nodeServerSolutionEntityService.SaveAsync(targetNodeServerSolution, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<NodeServerSolutionReadResultDto>()
            {
                Data = NodeServerSolutionMapper.ToNodeServerSolutionReadResultDto(targetNodeServerSolution)
            };
        }
        catch (Exception)
        {
            await dbContextTransactionAction.RollbackTransactionAsync(CancellationToken.None);
            
            throw;
        }
    }
}