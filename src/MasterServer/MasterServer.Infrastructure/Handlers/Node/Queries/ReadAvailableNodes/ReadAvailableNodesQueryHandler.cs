using FluentValidation;
using MasterServer.Application.Models.Dto.Node;
using MediatR;
using Shared.Common.Models.DTO.Base;

namespace MasterServer.Infrastructure.Handlers.Node.Queries.ReadAvailableNodes;

public class ReadAvailableNodesQueryHandler(
    IValidator<ReadAvailableNodesQuery> validator
) : IRequestHandler<ReadAvailableNodesQuery, ResponseBase<ReadNodeResponseBase>>
{
    public async Task<ResponseBase<ReadNodeResponseBase>> Handle(ReadAvailableNodesQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        try
        {
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}