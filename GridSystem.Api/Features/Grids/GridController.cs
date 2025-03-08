using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Features.Grids;

public class AppContext : DbContext;

[Route("api/grid")]
[ApiController]
public partial class GridController(IMediator Mediator) : ControllerBase;