namespace FrenchRevolution.Contracts.Models;

public class PagedList<T>
{
    private PagedList(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    
    public IReadOnlyList<T> Items { get; }
    public int Page { get;}
    public int PageSize { get;}
    public long TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
 
    public static PagedList<T> CreatePagedListAsync(
        IReadOnlyList<T> items, 
        int page, 
        int pageSize,
        int totalCount
        )
    {
        return new PagedList<T>(items, page, pageSize, totalCount);
    }
    
    public static PagedList<T> EmptyPagedList()
    {
        return new PagedList<T>([], 1, 0, 0);
    }
}