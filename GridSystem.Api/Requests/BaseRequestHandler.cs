using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.Requests;

public abstract class BaseRequestHandler<TRequest, TResponse>(DbContext dbContext)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly DbContext DbContext = dbContext;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}