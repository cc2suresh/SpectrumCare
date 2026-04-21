using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace SpectrumCare.BuildingBlocks.Web.Logging;

/// <summary>
/// Centralized Serilog configuration for all SpectrumCare services.
/// Every service must call AddSpectrumCareLogging() in Program.cs.
/// Structured logging is mandatory — never use Console.WriteLine or Debug.WriteLine.
/// Log levels: Verbose > Debug > Information > Warning > Error > Fatal.
/// Production minimum level: Information. Development minimum level: Debug.
/// Future: swap Serilog sinks to Azure Monitor without code changes.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures Serilog as the logging provider for the host.
    /// Reads additional configuration from appsettings.json Serilog section.
    /// Enriches all logs with MachineName, ThreadId, and Environment.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="serviceName">
    /// The name of the service — included in every log entry.
    /// Use the service assembly name e.g. "SpectrumCare.Identity".
    /// </param>
    public static IHostApplicationBuilder AddSpectrumCareLogging(
        this IHostApplicationBuilder builder,
        string serviceName)
    {
        builder.Services.AddSerilog((services, loggerConfig) =>
        {
            var environment = builder.Environment.EnvironmentName;
            var minimumLevel = builder.Environment.IsDevelopment()
                ? LogEventLevel.Debug
                : LogEventLevel.Information;

            loggerConfig
                .MinimumLevel.Is(minimumLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("ServiceName", serviceName)
                .Enrich.WithProperty("Version", GetServiceVersion())
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: $"logs/{serviceName}-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ServiceName}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
                .ReadFrom.Configuration(builder.Configuration);
        });

        return builder;
    }

    /// <summary>
    /// Adds request logging middleware to the pipeline.
    /// Logs HTTP method, path, status code, and duration for every request.
    /// Must be called after UseRouting() and before UseEndpoints().
    /// </summary>
    public static IApplicationBuilder UseSpectrumCareRequestLogging(
        this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

            options.GetLevel = (httpContext, elapsed, ex) => ex != null
                ? LogEventLevel.Error
                : httpContext.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : LogEventLevel.Information;

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            };
        });

        return app;
    }

    private static string GetServiceVersion()
    {
        return System.Reflection.Assembly
            .GetEntryAssembly()
            ?.GetName()
            ?.Version
            ?.ToString() ?? "1.0.0";
    }
}