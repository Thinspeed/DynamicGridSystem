using EFSelector;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    : BaseRequestHandler<GetSingleSelectColumnByIdQuery, GetSingleSelectColumnByIdQueryResponse>(dbContext)
{
    private static readonly Selector<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse> Selector =
        GetSingleSelectColumnByIdQueryResponse.Selector.Construct();
    
    public override async Task<GetSingleSelectColumnByIdQueryResponse> Handle(GetSingleSelectColumnByIdQuery request, CancellationToken cancellationToken)
    {
        var list = DbContext.Set<SingleSelectColumn>()
            .Where(c => c.Id == request.Id)
            .Select(x => x.Values);
        
        GetSingleSelectColumnByIdQueryResponse? response = await DbContext.Set<SingleSelectColumn>()
            .Where(x => x.Id == request.Id)
            .Select(Selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new Exception($"Single select column with Id {request.Id} does not exist");
        }
        
        return response;
    }
}

[UsedImplicitly]
public partial class GetSingleSelectColumnByIdQueryResponse
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public int Position { get; init; }
    
    public IEnumerable<SingleSelectValueModel> Values { get; init; }
    
    public static readonly EfSelector<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse> Selector =
        EfSelector.Declare<SingleSelectColumn, GetSingleSelectColumnByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Select(src => src.Position, dst => dst.Position)
            .SelectCollection(src => src.Values, dst => dst.Values, SingleSelectValueModel.Selector);
}

public partial class GetSingleSelectColumnByIdQueryResponse
{
    [UsedImplicitly]
    public class SingleSelectValueModel
    {
        public int Id { get; init; }
        
        public required string Value { get; init; }
        
        public static readonly EfSelector<SingleSelectValue, SingleSelectValueModel> Selector =
            EfSelector.Declare<SingleSelectValue, SingleSelectValueModel>()
                .Select(src => src.Id, dst => dst.Id)
                .Select(src => src.Value, dst => dst.Value);
    }
} 