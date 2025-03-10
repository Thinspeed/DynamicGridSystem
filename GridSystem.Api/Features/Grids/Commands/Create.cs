using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateGridCommandBody(string Name) : PostCommandBody;

public class CreateGridCommand : PostCommand<CreateGridCommandBody, int>;

public partial class GridController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGridCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateGridCommandHandler(ApplicationRwDbContext dbContext) : 
    BaseRequestHandler<CreateGridCommand, int>(dbContext)
{
    public override async Task<int> Handle(CreateGridCommand request, CancellationToken cancellationToken)
    {
        var grid = new Grid(request.Body.Name);

        await DbContext.Set<Grid>().AddAsync(grid, cancellationToken);
        
        await DbContext.SaveChangesAsync(cancellationToken);

        return grid.Id;
    }
}