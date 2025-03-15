using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateStringColumnCommandBody(
    string Name,
    int Position) : PostCommandBody;

public class CreateStringColumnCommand : PostCommand<CreateStringColumnCommandBody, int>
{
    [FromRoute]
    public int GridId { get; init; }
}

public partial class GridController
{
    [HttpPost("{GridId}/string")]
    public async Task<IActionResult> CreateStringColumn(CreateStringColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateStringColumnCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<CreateStringColumnCommand, int>(dbContext)
{
    public override async Task<int> Handle(CreateStringColumnCommand request, CancellationToken cancellationToken)
    {
        CreateStringColumnCommandBody body = request.Body;

        Grid grid = DbContext.Set<Grid>().FirstOrDefault(x => x.Id == request.GridId) ??
                    throw new Exception($"Grid with id {request.GridId} does not exist");

        StringColumn column = grid.AddStringColumn(body.Name, body.Position);

        await DbContext.SaveChangesAsync(cancellationToken);
        
        return column.Id; 
    }
}