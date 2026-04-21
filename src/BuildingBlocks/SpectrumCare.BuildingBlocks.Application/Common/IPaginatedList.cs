namespace SpectrumCare.BuildingBlocks.Application.Common;

/// <summary>
/// Represents a paginated list of items returned from a query.
/// Use this as the return type for all list-based queries across the platform.
/// Ensures consistent pagination behavior and metadata across all services.
/// </summary>
/// <typeparam name="T">The type of items in the paginated list.</typeparam>
public interface IPaginatedList<T>
{
    /// <summary>
    /// Gets the items for the current page.
    /// </summary>
    IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    bool HasPreviousPage { get; }

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    bool HasNextPage { get; }
}