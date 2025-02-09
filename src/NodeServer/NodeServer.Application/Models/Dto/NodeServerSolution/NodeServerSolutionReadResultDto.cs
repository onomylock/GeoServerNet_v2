using Shared.Common.Models.DTO.Base;

namespace NodeServer.Application.Models.Dto.NodeServerSolution;

public class NodeServerSolutionReadResultDto : EntityResponseBase
{
    public Guid MasterServerSolutionId { get; set; }
    public string DirectoryPath { get; set; }
    public string FileExePath { get; set; }
    public string DirectoryResultsPath { get; set; }
    public string ArgumentsMask { get; set; }
}