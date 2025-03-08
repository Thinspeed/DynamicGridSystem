using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Columns;

[UsedImplicitly]
public record UpdateNumericColumnCommandBody(
    int GridId,
    int ColumnId,
    string Name,
    int Position,
    int DecimalPlaces) : PutCommandBody;
    
public class UpdateNumericColumnCommand : PutCommand<UpdateNumericColumnCommandBody, Unit>;

public partial class ColumnController
{
    [HttpPut("numeric")]
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
        Grid grid = await dbContext.Set<Grid>().FirstOrDefaultAsync(grid => grid.Id == body.GridId, cancellationToken) ??
                    throw new Exception("Grid not found");
        
        grid.UpdateNumericColumn(body.ColumnId, body.Name, body.Position, body.DecimalPlaces);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}