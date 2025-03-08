using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Extensions;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Columns;

[UsedImplicitly]
public record CreateNumericColumnCommandBody(
    int GridId,
    string Name,
    int Position,
    int DecimalPlaces) : PostCommandBody;
    
public class CreateNumericColumnCommand : PostCommand<CreateNumericColumnCommandBody, int>;

public partial class ColumnController
{
    [HttpPost("numeric")]
    public async Task<IActionResult> CreateNumericColumn(CreateNumericColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateNumericColumnCommandHandler(ApplicationRwDbContext dbContext) : IRequestHandler<CreateNumericColumnCommand, int>
{
    public async Task<int> Handle(CreateNumericColumnCommand request, CancellationToken cancellationToken)
    {
        CreateNumericColumnCommandBody body = request.Body;
        
        Grid? grid = await dbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == body.GridId, cancellationToken);

        if (grid == null)
        {
            throw new ArgumentNullException($"Grid with {body.GridId} not found");
        }
        
        NumericColumn column =  grid.AddNumericColumn(body.Name, body.Position, body.DecimalPlaces);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return column.Id;
    }
}