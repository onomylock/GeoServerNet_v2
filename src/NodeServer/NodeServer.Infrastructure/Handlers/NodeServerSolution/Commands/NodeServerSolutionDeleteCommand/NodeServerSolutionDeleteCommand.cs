using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodeServer.Application.Models.Dto.NodeServerSolution;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerSolution.Commands.NodeServerSolutionDeleteCommand;

public class NodeServerSolutionDeleteCommand : NodeServerSolutionTargetRequestDto, IRequest<ResponseBase<OkResult>>;