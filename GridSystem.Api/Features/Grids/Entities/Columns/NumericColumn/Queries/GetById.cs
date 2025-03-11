using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetNumericColumnByIdQuery : GetByIdQuery<int, GetNumericColumnByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("numeric/{Id}")]
    public async Task<IActionResult> GetNumericColumnById(GetNumericColumnByIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetNumericColumnByIdQueryHandler(ApplicationRoDbContext dbContext) 
    : BaseRequestHandler<GetNumericColumnByIdQuery, GetNumericColumnByIdQueryResponse>(dbContext)
{
    private static Selector<NumericColumn, GetNumericColumnByIdQueryResponse> _selector =
        GetNumericColumnByIdQueryResponse.Selector;
    
    public override async Task<GetNumericColumnByIdQueryResponse> Handle(GetNumericColumnByIdQuery request, CancellationToken cancellationToken)
    {
        GetNumericColumnByIdQueryResponse? response = await DbContext.Set<NumericColumn>()
            .Where(x => x.Id == request.Id)
            .Select(_selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            throw new Exception($"No numeric column found with id: {request.Id}");
        }
        
        return response;
    }
}

[UsedImplicitly]
public class GetNumericColumnByIdQueryResponse
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int Position { get; init; }
    
    public int DecimalPlaces { get; init; }
    
    
    public static Selector<NumericColumn, GetNumericColumnByIdQueryResponse> Selector =
        EfSelector.Declare<NumericColumn, GetNumericColumnByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Select(src => src.Position, dst => dst.Position)
            .Select(src => src.DecimalPlaces, dst => dst.DecimalPlaces)
            .Construct();
}