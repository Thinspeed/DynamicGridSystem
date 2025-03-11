using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Extensions;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateNumericColumnCommandBody(
    string Name,
    int Position,
    int DecimalPlaces) : PostCommandBody;

public class CreateNumericColumnCommand : PostCommand<CreateNumericColumnCommandBody, int>
{
    [FromRoute] public int GridId { get; init; }
}

public partial class GridController
{
    [HttpPost("{GridId}/numeric")]
    public async Task<IActionResult> CreateNumericColumn(CreateNumericColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateNumericColumnCommandHandler(ApplicationRwDbContext dbContext) 
    : BaseRequestHandler<CreateNumericColumnCommand, int>(dbContext)
{
    public override async Task<int> Handle(CreateNumericColumnCommand request, CancellationToken cancellationToken)
    {
        CreateNumericColumnCommandBody body = request.Body;
        
        Grid? grid = await DbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken);

        if (grid == null)
        {
            throw new ArgumentNullException($"Grid with {request.GridId} not found");
        }
        
        NumericColumn column =  grid.AddNumericColumn(body.Name, body.Position, body.DecimalPlaces);
        
        await DbContext.SaveChangesAsync(cancellationToken);

        return column.Id;
    }
}