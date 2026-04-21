namespace SpectrumCare.BuildingBlocks.Web.Responses;

/// <summary>
/// Standard API response wrapper used across all services.
/// Ensures consistent response structure for all API endpoints.
/// </summary>
/// <typeparam name="T">The type of data returned.</typeparam>
public sealed class ApiResponse<T>
{
    private ApiResponse(bool isSuccess, T? data, string? message, IEnumerable<string>? errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        Errors = errors?.ToList() ?? new List<string>();
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>Gets whether the request was successful.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets the response data on success.</summary>
    public T? Data { get; }

    /// <summary>Gets the response message.</summary>
    public string? Message { get; }

    /// <summary>Gets the list of errors on failure.</summary>
    public IReadOnlyList<string> Errors { get; }

    /// <summary>Gets the UTC timestamp of the response.</summary>
    public DateTime Timestamp { get; }

    /// <summary>Creates a successful response with data.</summary>
    public static ApiResponse<T> Success(T data, string? message = null) =>
        new(true, data, message, null);

    /// <summary>Creates a failure response with errors.</summary>
    public static ApiResponse<T> Failure(string message, IEnumerable<string>? errors = null) =>
        new(false, default, message, errors);
}