using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Api.SievePreferences;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Grids;

public class GetSingleSelectValueByColumnIdQuery : GetQuery<GetSingleSelectValueByIdQueryResponse>
{
    [FromRoute] public int ColumnId { get; set; }
}

public partial class GridController
{
    [HttpGet("single-select/{ColumnId}/values")]
    public async Task<IActionResult> GetSingleSelectValueByColumnId(GetSingleSelectValueByColumnIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetSingleSelectValueByColumnIdQueryHandler(
    ApplicationRoDbContext dbContext, 
    AppSieveProcessor sieveProcessor
) : BaseRequestHandler<GetSingleSelectValueByColumnIdQuery, GetSingleSelectValueByIdQueryResponse,
        PagedList<GetSingleSelectValueByIdQueryResponse>>(dbContext, sieveProcessor)
{

    private static readonly Selector<SingleSelectValue, GetSingleSelectValueByIdQueryResponse> Selector =
        GetSingleSelectValueByIdQueryResponse.Selector.Construct();
    
    public override async Task<PagedList<GetSingleSelectValueByIdQueryResponse>> Handle(GetSingleSelectValueByColumnIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<SingleSelectValue> query = DbContext.Set<SingleSelectValue>().Where(x => x.SingleSelectColumnId == request.ColumnId);

        return await CreatePagedListAsync(
            query,
            request.SieveModel,
            Selector.Expression,
            cancellationToken);
    }
}