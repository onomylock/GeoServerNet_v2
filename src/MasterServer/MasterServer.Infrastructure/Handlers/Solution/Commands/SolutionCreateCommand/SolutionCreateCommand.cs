using MasterServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Solution.Commands.SolutionCreateCommand;

public class SolutionCreateCommand : IRequest<ResponseBase<OkResult>>
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string SolutionTypeAlias { get; set; }
}