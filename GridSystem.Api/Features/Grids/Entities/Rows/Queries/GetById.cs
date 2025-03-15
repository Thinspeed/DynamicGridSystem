using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Rows;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetRowByIdQuery : GetByIdQuery<int, GetRowByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("row/{Id}")]
    public async Task<IActionResult> GetRowById(GetRowByIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetRowByIdQueryHandler(ApplicationRoDbContext dbContext)
    : BaseRequestHandler<GetRowByIdQuery, GetRowByIdQueryResponse>(dbContext)
{
    private static readonly Selector<Row, GetRowByIdQueryResponse> Selector = GetRowByIdQueryResponse.Selector;
    
    public override async Task<GetRowByIdQueryResponse> Handle(GetRowByIdQuery request, CancellationToken cancellationToken)
    {
        GetRowByIdQueryResponse? response = await DbContext.Set<Row>()
            .Where(x => x.Id == request.Id)
            .Select(Selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new Exception($"Row with {request.Id} does not exist");
        }
        
        return response;
    }
}

[UsedImplicitly]
public class GetRowByIdQueryResponse
{
    public int Id { get; set; }
    
    public string Data { get; set; }
    
    public static readonly Selector<Row, GetRowByIdQueryResponse> Selector =
        EfSelector.Declare<Row, GetRowByIdQueryResponse>()
            .Select(src => src.Id, dest => dest.Id)
            .Select(src => src.DataAsString, dest => dest.Data)
            .Construct();
}