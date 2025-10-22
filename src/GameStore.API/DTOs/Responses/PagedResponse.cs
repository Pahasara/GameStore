namespace GameStore.DTOs.Responses;

public class PagedResponse<T>
{
    // Actual page data
    public IReadOnlyList<T> Items { get; set; } = [];

    // Pagination metadata
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    // Computed properties
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    // Actual count of returned items
    public int Count => Items.Count;

    // return start-end of total => "11-20 of 100"
    public string ItemsRange
    {
        get
        {
            if (TotalCount == 0) return "0-0 of 0";
            var start = (CurrentPage - 1) * PageSize + 1;
            var end = Math.Min(start + Count - 1, TotalCount);
            return $"{start}-{end} of {TotalCount}";
        }
    }

    // Factory method to easily create a paged response
    public static PagedResponse<T> Create(
        IReadOnlyList<T> items,
        int currentPage,
        int pageSize,
        int totalCount
    )
    {
        return new PagedResponse<T>
        {
            Items = items,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
