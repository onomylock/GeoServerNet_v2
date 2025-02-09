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

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionCreateCommand;

public class NodeServerSolutionCreateHandler(
    IValidator<NodeServerSolutionCreateCommand> validator,
    IDbContextTransactionAction dbContextTransactionAction,
    INodeServerSolutionEntityService nodeServerSolutionEntityService,
    HttpClient httpClient,
    IFileService fileService,
    IMinioService minioService
) : IRequestHandler<NodeServerSolutionCreateCommand, ResponseBase<NodeServerSolutionReadResultDto>>
{
    public async Task<ResponseBase<NodeServerSolutionReadResultDto>> Handle(NodeServerSolutionCreateCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        try
        {
            await dbContextTransactionAction.BeginTransactionAsync(cancellationToken);

            if (await nodeServerSolutionEntityService.GetByMasterServerSolutionIdAsync(request.MasterServerSolutionId,
                    true, cancellationToken) is { })
                throw new NodeServerSolutionAlredyExistsException();
            
            var uri = await minioService.GetFileUrl(request.FileName, request.BucketName, cancellationToken);
            
            var requestStream = await httpClient.GetStreamAsync(uri, cancellationToken);

            var solutionPath = await fileService.ExtractArchiveAsync(requestStream, FileHelper.BuildsPath, cancellationToken);
            
            var workingDirectory = FileHelper.CreateSolutionResultsPath(solutionPath);

            var fileExePath = Path.Combine(solutionPath, request.FileName);
            
            if (!File.Exists(Path.Combine(solutionPath, request.FileName)))
                throw new FileNotFoundException();
            
            var targetNodeServerSolution = new Domain.Entities.NodeServerSolution
            {
                MasterServerSolutionId = request.MasterServerSolutionId,
                DirectoryPath = solutionPath,
                FileExePath = fileExePath,
                DirectoryResultsPath = workingDirectory.ToString(),
                ArgumentsMask = request.ArgumentsMask,
            };
            
            await nodeServerSolutionEntityService.SaveAsync(targetNodeServerSolution, cancellationToken);
            
            await dbContextTransactionAction.CommitTransactionAsync(cancellationToken);

            return new ResponseBase<NodeServerSolutionReadResultDto>
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