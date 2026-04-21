using Microsoft.AspNetCore.Builder;
using SpectrumCare.BuildingBlocks.Web.Middleware;

namespace SpectrumCare.BuildingBlocks.Web.Extensions;

/// <summary>
/// Extension methods for registering BuildingBlocks middleware.
/// Call UseSpectrumCareExceptionHandling() in every service's Program.cs.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Registers the global exception handling middleware.
    /// Must be called before any other middleware registrations.
    /// </summary>
    public static IApplicationBuilder UseSpectrumCareExceptionHandling(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}