using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Api.SievePreferences;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetGridQuery : GetQuery<GetGridByIdQueryResponse>;

public partial class GridController
{
    [HttpGet]
    public async Task<IActionResult> Get(GetGridQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetGridQueryHandler(ApplicationRoDbContext dbContext, AppSieveProcessor sieveProcessor)
    : BaseRequestHandler<GetGridQuery, GetGridByIdQueryResponse, PagedList<GetGridByIdQueryResponse>>(dbContext, sieveProcessor)
{
    private static readonly Selector<Grid, GetGridByIdQueryResponse> Selector = GetGridByIdQueryResponse.Selector;
    
    public override async Task<PagedList<GetGridByIdQueryResponse>> Handle(GetGridQuery request, CancellationToken cancellationToken)
    {
        IQueryable<GetGridByIdQueryResponse> query = DbContext.Set<Grid>().Select(Selector.Expression);

        return await CreatePagedListAsync(query, request.SieveModel, cancellationToken);
    }
}