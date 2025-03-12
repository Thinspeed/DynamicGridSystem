using GridSystem.Api.SievePreferences;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace GridSystem.Api.Requests;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly DbContext DbContext;

    protected BaseRequestHandler(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class BaseRequestHandler<TRequest, TBaseType, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse> 
    where TResponse : PagedList<TBaseType>
{
    protected readonly DbContext DbContext;
    
    private readonly AppSieveProcessor _sieveProcessor;

    public BaseRequestHandler(DbContext dbContext, AppSieveProcessor sieveProcessor)
    {
        DbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }
    
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    protected async Task<PagedList<TBaseType>> CreatePagedListAsync(IQueryable<TBaseType> query, SieveModel sieveModel, CancellationToken cancellationToken)
    {
        ValidateAndSetSieveModel(sieveModel);
        
        int totalCount = await query.CountAsync(cancellationToken);
        
        IQueryable<TBaseType> sieveQuery = _sieveProcessor!.Apply(sieveModel, query);

        return new PagedList<TBaseType>()
        {
            TotalCount = totalCount,
            Page = sieveModel.Page!.Value,
            PageSize = sieveModel.PageSize!.Value,
            TotalPages = (int)Math.Ceiling((double)totalCount / sieveModel.PageSize!.Value),
            Data = await sieveQuery.ToListAsync(cancellationToken)
        };
    }

    private void ValidateAndSetSieveModel(SieveModel sieveModel)
    {
        sieveModel.PageSize ??= _sieveProcessor!.DefaultPageSize;
        sieveModel.Page ??= _sieveProcessor!.DefaultPage;
    }
}