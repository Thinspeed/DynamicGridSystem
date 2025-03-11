using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GridSystem.Api.Requests;

public abstract record PostCommandBody;

public abstract record PutCommandBody;

public abstract class PostCommand<TBody> : IRequest
{
    [FromBody]
    public PostCommandBody Body { get; init; }
}

public abstract class PostCommand<TBody, TResponse> : IRequest<TResponse>
    where TBody : PostCommandBody
{
    [FromBody]
    public TBody Body { get; init; }
}

public abstract class PutCommand<TBody, TResponse> : IRequest<TResponse>
    where TBody : PutCommandBody
{
    [FromBody]
    public TBody Body { get; init; }
}

public abstract class PutCommandWithId<TId, TBody, TResponse> : IRequest<TResponse>
    where TBody : PutCommandBody
{
    [FromRoute]
    public TId Id { get; init; }
    
    [FromBody]
    public TBody Body { get; init; }
}

public abstract class DeleteCommand<TId, TResponse> : IRequest<TResponse>
{
    [FromRoute]
    public TId Id { get; init; }
}

public abstract class GetByIdQuery<TId, TResponse> : IRequest<TResponse>
{
    [FromRoute]
    public TId Id { get; init; }
}