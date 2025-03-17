using System.Text.Json;
using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Rows;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateRowCommandBody : PostCommandBody;

public class CreateRowCommand : PostCommand<CreateRowCommandBody, int>
{
    [FromRoute]
    public int GridId { get; init; }
}

public partial class GridController
{
    [HttpPost("{GridId}/row")]
    public async Task<IActionResult> CreateRow(CreateRowCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateRowCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<CreateRowCommand, int>(dbContext)
{
    public override async Task<int> Handle(CreateRowCommand request, CancellationToken cancellationToken)
    {
        Grid grid = await DbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken) 
                    ?? throw new Exception($"Grid with Id {request.GridId} does not exist");

        Row row = grid.AddRow();
        
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return row.Id;
    }
}