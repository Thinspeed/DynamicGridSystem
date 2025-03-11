using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record CreateSingleSelectValueCommandBody(string Value) : PostCommandBody;

public class CreateSingleSelectValueCommand : PostCommand<CreateSingleSelectValueCommandBody, int>
{
    [FromRoute] public int ColumnId { get; set; }
}

public partial class GridController
{
    [HttpPost("single-select/{Id}/value")]
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
        
        var entity = new SingleSelectValue(body.Value, request.ColumnId);
        
        await dbContext.Set<SingleSelectValue>().AddAsync(entity, cancellationToken);

        return entity.Id;
    }
}