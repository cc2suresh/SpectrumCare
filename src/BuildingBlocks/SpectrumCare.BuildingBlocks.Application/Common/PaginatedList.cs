namespace SpectrumCare.BuildingBlocks.Application.Common;

/// <summary>
/// Concrete implementation of IPaginatedList{T}.
/// Use the static Create method to instantiate from any IQueryable or IEnumerable.
/// This class is used as the standard return type for all paginated queries.
/// Example: return PaginatedList{ClientResponse}.Create(clients, pageNumber, pageSize);
/// </summary>
/// <typeparam name="T">The type of items in the paginated list.</typeparam>
public class PaginatedList<T> : IPaginatedList<T>
{
    private PaginatedList(
        IReadOnlyList<T> items,
        int totalCount,
        int pageNumber,
        int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    /// <inheritdoc/>
    public IReadOnlyList<T> Items { get; }

    /// <inheritdoc/>
    public int PageNumber { get; }

    /// <inheritdoc/>
    public int PageSize { get; }

    /// <inheritdoc/>
    public int TotalCount { get; }

    /// <inheritdoc/>
    public int TotalPages { get; }

    /// <inheritdoc/>
    public bool HasPreviousPage => PageNumber > 1;

    /// <inheritdoc/>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new PaginatedList from an in-memory list.
    /// Use this when data is already loaded into memory.
    /// </summary>
    /// <param name="source">The full list of items.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    public static PaginatedList<T> Create(
        IEnumerable<T> source,
        int pageNumber,
        int pageSize)
    {
        var list = source.ToList();
        var totalCount = list.Count;
        var items = list
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Creates a new PaginatedList when total count is already known.
    /// Use this with Dapper queries where count and data are fetched separately.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    public static PaginatedList<T> Create(
        IEnumerable<T> items,
        int totalCount,
        int pageNumber,
        int pageSize)
    {
        return new PaginatedList<T>(
            items.ToList(),
            totalCount,
            pageNumber,
            pageSize);
    }
}