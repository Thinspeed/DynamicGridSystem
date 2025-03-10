using EntityFramework.Preferences;
using GridSystem.Api.Requests;
using GridSystem.Domain.Grids;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

[UsedImplicitly]
public record UpdateGridCommandBody(string Name) : PutCommandBody;

public class UpdateGridCommand : PutCommandWithId<UpdateGridCommandBody, Unit>;

public partial class GridController
{
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(UpdateGridCommand request)
    {
        return Ok(await Mediator.Send(request));
    }
}

[UsedImplicitly]
public class UpdateGridCommandHandler(ApplicationRwDbContext dbContext) : IRequestHandler<UpdateGridCommand, Unit>
{
    public async Task<Unit> Handle(UpdateGridCommand request, CancellationToken cancellationToken)
    {
        UpdateGridCommandBody body = request.Body;
        
        Grid? entity = await dbContext.Set<Grid>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Grid with id {request.Id} does not exist");
        }
        
        entity.Update(body.Name);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}