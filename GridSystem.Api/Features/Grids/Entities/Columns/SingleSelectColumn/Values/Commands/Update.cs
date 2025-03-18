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
public record UpdateSingleSelectValueCommandBody(string Value) : PutCommandBody;

public class UpdateSingleSelectValueCommand : PutCommandWithId<int, UpdateSingleSelectValueCommandBody, Unit>
{
    [FromRoute] public int GridId { get; init; }
    [FromRoute] public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpPut("{GridId}/single-select/{ColumnId}/value/{Id}")]
    public async Task<IActionResult> UpdateSingleSelectValue(UpdateSingleSelectValueCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateSingleSelectValueCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<UpdateSingleSelectValueCommand, Unit>(dbContext)
{
    public override async Task<Unit> Handle(UpdateSingleSelectValueCommand request, CancellationToken cancellationToken)
    {
        UpdateSingleSelectValueCommandBody body = request.Body;
        
        Grid? grid = await DbContext.Set<Grid>()
            .Include(x => x.Columns.AsQueryable()
                .Where(c => c.Id == request.ColumnId))
                .ThenInclude(x => (x as SingleSelectColumn)!.Values)
            .Include(x => x.Rows.AsQueryable()
                .Where(r => r.Data.RootElement.GetProperty(request.ColumnId.ToString()).GetProperty(nameof(SingleSelectValue.Id)).GetInt32() == request.Id))
            .FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken);

        if (grid is null)
        {
            throw new Exception($"Grid with id {request.GridId} does not exist");
        }
        
        grid.UpdateSingleSelectValue(request.ColumnId, request.Id, body.Value);

        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}