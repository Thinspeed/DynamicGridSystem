using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetSingleSelectValueByIdQuery : GetByIdQuery<int, GetSingleSelectValueByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("single-select/value/{Id}")]
    public async Task<IActionResult> GetSingleSelectValueById(GetSingleSelectValueByIdQueryResponse request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetSingleSelectValueByIdQueryHandler(ApplicationRoDbContext dbContext)
    : BaseRequestHandler<GetSingleSelectValueByIdQuery, GetSingleSelectValueByIdQueryResponse>(dbContext)
{
    private static readonly Selector<SingleSelectValue, GetSingleSelectValueByIdQueryResponse> Selector =
        GetSingleSelectValueByIdQueryResponse.Selector;
    
    public override async Task<GetSingleSelectValueByIdQueryResponse> Handle(GetSingleSelectValueByIdQuery request, CancellationToken cancellationToken)
    {
        GetSingleSelectValueByIdQueryResponse? response = await DbContext.Set<SingleSelectValue>()
            .Select(Selector.Expression)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (response is null)
        {
            throw new Exception($"Single-select value with id {request.Id} does not exist");
        }

        return response;
    }
}

[UsedImplicitly]
public class GetSingleSelectValueByIdQueryResponse
{
    public int Id { get; init; }
        
    public required string Value { get; init; }
        
    public static readonly Selector<SingleSelectValue, GetSingleSelectValueByIdQueryResponse> Selector = 
        EfSelector.Declare<SingleSelectValue, GetSingleSelectValueByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Value, dst => dst.Value)
            .Construct();
}