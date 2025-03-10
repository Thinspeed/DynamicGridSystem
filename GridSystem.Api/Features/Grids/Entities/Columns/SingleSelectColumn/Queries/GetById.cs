using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSolution.Selector;

namespace GridSystem.Api.Features.Grids;

public class GetSingleSelectColumnByIdQuery : GetByIdQuery<int, GetSingleSelectColumnByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("single-select/{Id}")]
    public async Task<IActionResult> GetSingleSelectColumnById(GetSingleSelectColumnByIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class GetSingleSelectColumnByIdQueryHandler(ApplicationRoDbContext dbContext)
    : IRequestHandler<GetSingleSelectColumnByIdQuery, GetSingleSelectColumnByIdQueryResponse>
{
    private static Selector<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse> _selector =
        GetSingleSelectColumnByIdQueryResponse.Selector;
    
    public async Task<GetSingleSelectColumnByIdQueryResponse> Handle(GetSingleSelectColumnByIdQuery request, CancellationToken cancellationToken)
    {
        GetSingleSelectColumnByIdQueryResponse? response = await dbContext.Set<SingleSelectColumn>()
            .Where(x => x.Id == request.Id)
            .Select(_selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new Exception($"Single select column with Id {request.Id} does not exist");
        }
        
        return response;
    }
}

[UsedImplicitly]
public class GetSingleSelectColumnByIdQueryResponse
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int Position { get; init; }
    
    public List<string> Values { get; init; }
    
    public static Selector<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse> Selector =
        EfSelector.Declare<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Select(src => src.Position, dst => dst.Position)
            .Select(src => src.Values, dst => dst.Values)
            .Construct();
}