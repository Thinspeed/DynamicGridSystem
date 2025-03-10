using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Columns;

[UsedImplicitly]
public record UpdateSingleSelectColumnCommandBody(
    int GridId,
    int ColumnId,
    string Name,
    int Position,
    List<string> Values) : PutCommandBody;
    
public class UpdateSingleSelectColumnCommand : PutCommand<UpdateSingleSelectColumnCommandBody, Unit>;

public partial class ColumnController
{
    [HttpPut("single-select")]
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
        
        Grid grid = await dbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == body.GridId, cancellationToken) ??
                    throw new Exception($"Grid with id {body.GridId} does not exist");
        
        grid.UpdateSingleSelectColumn(body.ColumnId, body.Name, body.Position, body.Values);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value; 
    }
}