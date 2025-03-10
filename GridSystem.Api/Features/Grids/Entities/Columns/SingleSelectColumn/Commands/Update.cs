using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateSingleSelectColumnCommandBody(
    string Name,
    int Position,
    List<string> Values) : PutCommandBody;

public class UpdateSingleSelectColumnCommand : PutCommand<UpdateSingleSelectColumnCommandBody, Unit>
{
    [FromRoute] public int GridId { get; init; }
    
    [FromRoute] public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpPut("{GridId}/single-select/{ColumnId}")]
    public async Task<IActionResult> CreateSingleSelectColumn(UpdateSingleSelectColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateSingleSelectColumnCommandHandler(ApplicationRwDbContext dbContext)
    : IRequestHandler<UpdateSingleSelectColumnCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSingleSelectColumnCommand request, CancellationToken cancellationToken)
    {
        UpdateSingleSelectColumnCommandBody body = request.Body;
        
        Grid grid = await dbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken) ??
                    throw new Exception($"Grid with id {request.GridId} does not exist");
        
        grid.UpdateSingleSelectColumn(request.ColumnId, body.Name, body.Position, body.Values);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value; 
    }
}