using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

public class RemoveSingleSelectValueCommand : DeleteCommand<int, Unit>
{
    [FromRoute] public int GridId { get; init; }
    [FromRoute] public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpDelete("{GridId}/single-select/{ColumnId}/value/{Id}")]
    public async Task<IActionResult> RemoveSingleSelectValue(RemoveSingleSelectValueCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}

[UsedImplicitly]
public class RemoveSingleSelectValueCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<RemoveSingleSelectValueCommand, Unit>(dbContext)
{
    public override async Task<Unit> Handle(RemoveSingleSelectValueCommand request, CancellationToken cancellationToken)
    {
        Grid? grid = await DbContext.Set<Grid>()
            .Include(x => x.Columns.AsQueryable().Where(c => c.Id == request.ColumnId))
                .ThenInclude(x => (x as SingleSelectColumn)!.Values)
            .Include(x => x.Rows.AsQueryable()
                .Where(r => r.Data.RootElement.GetProperty(request.ColumnId.ToString()).GetProperty(nameof(SingleSelectValue.Id)).GetInt32() == request.Id))
            .FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken);

        if (grid is null)
        {
            throw new Exception($"Grid with id {request.GridId} does not exist");
        }
        
        grid.RemoveSingleSelectValue(request.ColumnId, request.Id);

        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}