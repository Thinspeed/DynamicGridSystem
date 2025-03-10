using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateSingleSelectColumnCommandBody(
    string Name,
    int Position,
    List<string> Values) : PostCommandBody;

public class CreateSingleSelectColumnCommand : PostCommand<CreateSingleSelectColumnCommandBody, int>
{
    [FromRoute] public int GridId { get; init; }
}

public partial class GridController
{
    [HttpPost("{GridId}/single-select")]
    public async Task<IActionResult> CreateSingleSelectColumn(CreateSingleSelectColumnCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class CreateSingleSelectColumnCommandHandler(ApplicationRwDbContext dbContext)
    : IRequestHandler<CreateSingleSelectColumnCommand, int>
{
    public async Task<int> Handle(CreateSingleSelectColumnCommand request, CancellationToken cancellationToken)
    {
        CreateSingleSelectColumnCommandBody body = request.Body;

        Grid grid = await dbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == request.GridId, cancellationToken) ??
                    throw new Exception($"Grid with id {request.GridId} does not exist");
        
        SingleSelectColumn column = grid.AddSingleSelectColumn(body.Name, body.Position, body.Values);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return column.Id;
    }
}