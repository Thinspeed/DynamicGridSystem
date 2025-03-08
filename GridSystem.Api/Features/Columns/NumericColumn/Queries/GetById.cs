using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSolution.Selector;

namespace GridSystem.Api.Features.Columns;

public class GetByIdNumericColumnQuery : GetByIdQuery<int, GetByIdNumericColumnQueryResponse>;

public partial class ColumnController
{
    [HttpGet("numeric/{Id}")]
    public async Task<IActionResult> GetById(GetByIdNumericColumnQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetByIdNumericColumnQueryHandler(ApplicationRoDbContext dbContext) : IRequestHandler<GetByIdNumericColumnQuery, GetByIdNumericColumnQueryResponse>
{
    private static Selector<NumericColumn, GetByIdNumericColumnQueryResponse> _selector =
        GetByIdNumericColumnQueryResponse.Selector;
    
    public async Task<GetByIdNumericColumnQueryResponse> Handle(GetByIdNumericColumnQuery request, CancellationToken cancellationToken)
    {
        GetByIdNumericColumnQueryResponse? response = await dbContext.Set<NumericColumn>()
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
public class GetByIdNumericColumnQueryResponse
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int DecimalPlaces { get; init; }
    
    
    public static Selector<NumericColumn, GetByIdNumericColumnQueryResponse> Selector =
        EfSelector.Declare<NumericColumn, GetByIdNumericColumnQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Select(src => src.DecimalPlaces, dst => dst.DecimalPlaces)
            .Construct();
}