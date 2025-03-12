namespace GridSystem.Api.Requests;

public class PagedList<T>
{
    public required int TotalCount { get; init; }
    
    public required int? PageSize { get; init; }
    
    public required int? Page { get; init; }
    
    public required int? TotalPages { get; init; }
    
    public required List<T> Data { get; init; }
}