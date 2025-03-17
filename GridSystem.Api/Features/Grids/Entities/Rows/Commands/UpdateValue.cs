using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateRowValueCommandBody(
    int ColumnId,
    string Value) : PutCommandBody;

public class UpdateRowValueCommand : PutCommand<UpdateRowValueCommandBody, Unit>
{
    [FromRoute]
    public int GridId { get; init; }
    
    [FromRoute]
    public int RowId { get; init; }
}

public partial class GridController
{
    [HttpPut("{GridId}/row/{RowId}/value")]
    public async Task<IActionResult> UpdateRowValue(UpdateRowValueCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateRowValueCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<UpdateRowValueCommand, Unit>(dbContext)
{
    public override async Task<Unit> Handle(UpdateRowValueCommand request, CancellationToken cancellationToken)
    {
        UpdateRowValueCommandBody body = request.Body;
        
        Grid? grid = await DbContext.Set<Grid>()
            .Include(x => (x.Columns).AsQueryable().Where(c => c.Id == body.ColumnId))
                .ThenInclude(x => (x as SingleSelectColumn)!.Values)
            .Include(x => x.Rows.Where(r => r.Id == request.RowId))
            .FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken);

        if (grid is null)
        {
            throw new Exception($"Grid with Id {request.GridId} doest not exist");
        }
        
        grid.UpdateRow(request.RowId, body.ColumnId, body.Value);

        await DbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}