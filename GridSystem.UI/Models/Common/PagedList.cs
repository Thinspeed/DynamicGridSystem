using System.Collections;

namespace GridSystem.UI.Models.Common;

public class PagedList<T>
{
    public int TotalCount { get; init; }
    
    public int PageSize { get; init; }
    
    public int Page { get; init; }
    
    public int TotalPages { get; init; }
    
    public required List<T> Data { get; init; }
}