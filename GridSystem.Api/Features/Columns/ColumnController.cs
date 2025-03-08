using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Features.Columns;

[Route("api/column")]
[ApiController]
public partial class ColumnController(IMediator Mediator) : ControllerBase;