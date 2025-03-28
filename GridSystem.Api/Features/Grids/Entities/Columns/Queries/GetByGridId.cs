using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Api.SievePreferences;
using GridSystem.Domain.Grids.Columns;
using GridSystem.Domain.Grids.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public class GetColumnsByGridIdQuery : GetQuery<GetColumnsByGridIdQueryResponse>
{
    [FromRoute] public int GridId { get; init; }
}

public partial class GridController
{
    [HttpGet("{GridId}/columns")]
    public async Task<IActionResult> GetColumnsByGridId(GetColumnsByGridIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetColumnsByGridIdQueryHandler(ApplicationRoDbContext dbContext, AppSieveProcessor sieveProcessor)
    : BaseRequestHandler<GetColumnsByGridIdQuery, GetColumnsByGridIdQueryResponse,
        PagedList<GetColumnsByGridIdQueryResponse>>(dbContext, sieveProcessor)
{
    private static readonly Selector<Column, GetColumnsByGridIdQueryResponse> Selector =
        GetColumnsByGridIdQueryResponse.Selector.Construct();
    
    public override async Task<PagedList<GetColumnsByGridIdQueryResponse>> Handle(GetColumnsByGridIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Column> query = DbContext.Set<Column>().Where(c => c.GridId == request.GridId);
        
        return await CreatePagedListAsync(
            query, 
            request.SieveModel,
            Selector.Expression,
            cancellationToken);
    }
}

[UsedImplicitly]
public class GetColumnsByGridIdQueryResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public ColumnTypeModel Type { get; set; }
    
    public int Position { get; set; }

    public static readonly EfSelector<Column, GetColumnsByGridIdQueryResponse> Selector =
        EfSelector.Declare<Column, GetColumnsByGridIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Select(src => src.Type, dst => dst.Type, ColumnTypeModel.Selector)
            .Select(src => src.Position, dst => dst.Position);
    
    public class ColumnTypeModel
    {
        public string Name { get; set; }

        //todo подумать про маппинг с кастом, чтобы передавать Id(ColumnTypeEnum)
        public static readonly EfSelector<ColumnType, ColumnTypeModel> Selector =
            EfSelector.Declare<ColumnType, ColumnTypeModel>()
                .Select(src => src.Name, dst => dst.Name);
    }
}