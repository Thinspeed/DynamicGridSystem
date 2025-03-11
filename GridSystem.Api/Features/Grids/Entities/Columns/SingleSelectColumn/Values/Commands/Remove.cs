using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids.Columns;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

public class RemoveSingleSelectValueCommand : DeleteCommand<int, Unit>;

public partial class GridController
{
    [HttpDelete("single-select/value/{Id}")]
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
        SingleSelectValue? entity = await DbContext.Set<SingleSelectValue>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"Single-select value with id {request.Id} does not exist");
        }

        DbContext.Set<SingleSelectValue>().Remove(entity);
        
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}