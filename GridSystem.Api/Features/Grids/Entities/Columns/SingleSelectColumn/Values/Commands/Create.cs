using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateSingleSelectValueCommandBody(string Value) : PostCommandBody;

public class CreateSingleSelectValueCommand : PostCommand<CreateSingleSelectValueCommandBody, int>
{
    [FromRoute] public int GridId { get; init; }
    [FromRoute] public int ColumnId { get; init; }
}

public partial class GridController
{
    [HttpPost("{GridId}/single-select/{ColumnId}/value")]
    public async Task<IActionResult> CreateSingleSelectValue(CreateSingleSelectValueCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateSingleSelectValueCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<CreateSingleSelectValueCommand, int>(dbContext)
{
    public override async Task<int> Handle(CreateSingleSelectValueCommand request, CancellationToken cancellationToken)
    {
        CreateSingleSelectValueCommandBody body = request.Body;

        Grid? grid = await DbContext.Set<Grid>()
            .Include(x => x.Columns.Where(c => c.Id == request.ColumnId))
            .FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken);

        if (grid is null)
        {
            throw new Exception($"Grid with id {request.GridId} does not exist");
        }

        SingleSelectValue entity = grid.AddSingleSelectValue(request.ColumnId, body.Value);

        await DbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}