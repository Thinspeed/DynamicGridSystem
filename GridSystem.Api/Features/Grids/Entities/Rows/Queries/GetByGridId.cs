using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Api.SievePreferences;
using GridSystem.Domain.Grids.Rows;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetRowsByGridIdQuery : GetQuery<GetRowByIdQueryResponse>
{
    [FromRoute]
    public int GridId { get; init; }
}

public partial class GridController
{
    [HttpGet("{GridId}/row")]
    public async Task<IActionResult> GetRowsByGridId(GetRowsByGridIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetRowsByGridIdQueryHandler(ApplicationRoDbContext dbContext, AppSieveProcessor sieveProcessor)
    : BaseRequestHandler<GetRowsByGridIdQuery, GetRowByIdQueryResponse, PagedList<GetRowByIdQueryResponse>>(dbContext,
        sieveProcessor)
{
    private static readonly Selector<Row, GetRowByIdQueryResponse> Selector = GetRowByIdQueryResponse.Selector;
    
    public override async Task<PagedList<GetRowByIdQueryResponse>> Handle(GetRowsByGridIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Row> query = DbContext.Set<Row>();
        
        return await CreatePagedListAsync(
            query, 
            request.SieveModel,
            Selector.Expression,
            cancellationToken);
    }
}