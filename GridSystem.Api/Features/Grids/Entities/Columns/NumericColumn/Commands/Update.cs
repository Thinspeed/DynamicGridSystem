using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateNumericColumnCommandBody(
    string Name,
    int Position,
    int DecimalPlaces) : PutCommandBody;

public class UpdateNumericColumnCommand : PutCommand<UpdateNumericColumnCommandBody, Unit>
{
    [FromRoute] public int GridId { get; init; }
    
    [FromRoute] public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpPut("{GridId}/numeric/{ColumnId}")]
    public async Task<IActionResult> UpdateNumericColumnAsync(UpdateNumericColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateNumericColumnCommandHandler(ApplicationRwDbContext dbContext)
    : IRequestHandler<UpdateNumericColumnCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNumericColumnCommand request, CancellationToken cancellationToken)
    {
        UpdateNumericColumnCommandBody body = request.Body;
        Grid grid = await dbContext.Set<Grid>()
                        .Include(x => x.Columns.Where(c => c.Id == request.ColumnId))
                        .FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken) 
                    ?? throw new Exception($"Grid with id {request.GridId} not found");
        
        grid.UpdateNumericColumn(request.ColumnId, body.Name, body.Position, body.DecimalPlaces);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}