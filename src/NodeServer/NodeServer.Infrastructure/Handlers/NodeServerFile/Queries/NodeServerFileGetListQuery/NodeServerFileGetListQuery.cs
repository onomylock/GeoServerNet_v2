using MediatR;
using NodeServer.Application.Models.Dto.NodeServerJob;
using Shared.Common.Models.DTO.Base;

namespace NodeServer.Infrastructure.Handlers.NodeServerFile.Queries.NodeServerFileGetListQuery;

public class NodeServerFileGetListQuery : IRequest<ResponseBase<NodeServerJobReadResultDto>>
{
}