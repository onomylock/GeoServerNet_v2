using MasterServer.Application.Models.Dto.Solution;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionCreateCommand;

public class SolutionCreateCommand : IRequest<ResponseBase<SolutionReadResultDto>>
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string SolutionTypeAlias { get; set; }
}