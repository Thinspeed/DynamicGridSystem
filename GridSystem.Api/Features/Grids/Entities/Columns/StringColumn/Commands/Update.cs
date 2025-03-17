using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateStringColumnCommandBody(
    string Name,
    int Position) : PutCommandBody;

public class UpdateStringColumnCommand : PutCommand<UpdateStringColumnCommandBody, Unit>
{
    [FromRoute]
    public int GridId { get; init; }
    
    [FromRoute]
    public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpPut("{GridId}/string/{ColumnId}")]
    public async Task<IActionResult> UpdateStringColumn(UpdateStringColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateStringColumnCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<UpdateStringColumnCommand, Unit>(dbContext)
{
    public override async Task<Unit> Handle(UpdateStringColumnCommand request, CancellationToken cancellationToken)
    {
        UpdateStringColumnCommandBody body = request.Body;

        Grid grid = await DbContext.Set<Grid>()
                        .Include(x => x.Columns.Where(c => c.Id == request.ColumnId))
                        .FirstOrDefaultAsync(cancellationToken) 
                    ?? throw new Exception($"Grid with Id {request.GridId} doest not exist");
        
        grid.UpdateStringColumn(request.ColumnId, body.Name, body.Position);
        
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}