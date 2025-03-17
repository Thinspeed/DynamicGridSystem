using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateSingleSelectValueCommandBody(string Value) : PutCommandBody;

public class UpdateSingleSelectValueCommand : PutCommandWithId<int, UpdateSingleSelectValueCommandBody, Unit>;

public partial class GridController
{
    [HttpPost("single-select/value/{Id}")]
    public async Task<IActionResult> UpdateSingleSelectValue(UpdateSingleSelectValueCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateSingleSelectValueCommandHandler(ApplicationRwDbContext dbContext)
    : BaseRequestHandler<UpdateSingleSelectValueCommand, Unit>(dbContext)
{
    //todo обновлять нужно через grid, чтобы сразу обновить все ряды в которых использовалось значение из single select
    public override async Task<Unit> Handle(UpdateSingleSelectValueCommand request, CancellationToken cancellationToken)
    {
        UpdateSingleSelectValueCommandBody body = request.Body;

        SingleSelectValue? entity = await DbContext.Set<SingleSelectValue>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"Single-select value with id {request.Id} does not exist");
        }
        
        entity.Value = body.Value;

        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}