using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Grids;

public record CreateGridCommandBody(string Name) : PostCommandBody;

public class CreateGridCommand : PostCommand<CreateGridCommandBody, int>;

public partial class GridController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGridCommand request)
    {
        return Ok(Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateGridCommandHandler(ApplicationRwDbContext dbContext) : IRequestHandler<CreateGridCommand, int>
{
    public async Task<int> Handle(CreateGridCommand request, CancellationToken cancellationToken)
    {
        var grid = new Grid(request.Body.Name);

        await dbContext.Set<Grid>().AddAsync(grid, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return grid.Id;
    }
}