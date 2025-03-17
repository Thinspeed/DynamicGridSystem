using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

public class GetStringColumnByIdQuery : GetByIdQuery<int, GetStringColumnByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("string/{Id}")]
    public async Task<IActionResult> GetStringColumnById(GetStringColumnByIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetStringColumnByIdQueryHandler(ApplicationRoDbContext dbContext)
    : BaseRequestHandler<GetStringColumnByIdQuery, GetStringColumnByIdQueryResponse>(dbContext)
{
    private static readonly Selector<StringColumn, GetStringColumnByIdQueryResponse> Selector = 
        GetStringColumnByIdQueryResponse.Selector;
    
    public override async Task<GetStringColumnByIdQueryResponse> Handle(GetStringColumnByIdQuery request, CancellationToken cancellationToken)
    {
        GetStringColumnByIdQueryResponse? response = await DbContext.Set<StringColumn>()
            .Where(x => x.Id == request.Id)
            .Select(Selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new Exception($"Column with Id {request.Id} does not exist");
        }
        
        return response;
    }
}

[UsedImplicitly]
public class GetStringColumnByIdQueryResponse
{
    public int Id { get; init; }
    
    public required string Name { get; init; }

    public static readonly Selector<StringColumn, GetStringColumnByIdQueryResponse> Selector =
        EfSelector.Declare<StringColumn, GetStringColumnByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Construct();
}