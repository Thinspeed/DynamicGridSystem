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

public class GetGridByIdQuery : GetByIdQuery<int, GetGridByIdQueryResponse>;

public partial class GridController
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(GetGridByIdQuery request)
    {
        return Ok(await Mediator.Send(request));
    }
}

public class GetGridByIdQueryHandler(ApplicationRoDbContext dbContext)
    : BaseRequestHandler<GetGridByIdQuery, GetGridByIdQueryResponse>(dbContext)
{
    private static Selector<Grid, GetGridByIdQueryResponse> _selector = GetGridByIdQueryResponse.Selector;
    
    public override async Task<GetGridByIdQueryResponse> Handle(GetGridByIdQuery request, CancellationToken cancellationToken)
    {
        GetGridByIdQueryResponse? response = await DbContext.Set<Grid>()
            .Where(x => x.Id == request.Id)
            .Select(_selector.Expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            throw new Exception($"Grid with {request.Id} does not exist");
        }
        
        return response;
    }
}

[UsedImplicitly]
public class GetGridByIdQueryResponse
{
    public int Id { get; init; }
    
    public required string Name { get; init; }
    
    public static Selector<Grid, GetGridByIdQueryResponse> Selector = EfSelector.Declare<Grid, GetGridByIdQueryResponse>()
            .Select(src => src.Id, dst => dst.Id)
            .Select(src => src.Name, dst => dst.Name)
            .Construct();
}