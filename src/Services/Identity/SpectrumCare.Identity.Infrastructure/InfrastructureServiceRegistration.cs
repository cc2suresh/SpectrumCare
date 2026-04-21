using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;
using SpectrumCare.Identity.Domain.Repositories;
using SpectrumCare.Identity.Domain.Services;
using SpectrumCare.Identity.Infrastructure.Authentication;
using SpectrumCare.Identity.Infrastructure.Persistence;
using SpectrumCare.Identity.Infrastructure.Persistence.Repositories;
using SpectrumCare.Identity.Infrastructure.Services;

namespace SpectrumCare.Identity.Infrastructure;

/// <summary>
/// Registers all Infrastructure layer dependencies for the Identity service.
/// Call AddIdentityInfrastructure() in Program.cs.
/// Keeps Program.cs clean and infrastructure concerns isolated.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Registers EF Core, repositories, JWT, and service implementations.
    /// </summary>
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("IdentityDb"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));

        // JWT Settings
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        // Domain Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // BuildingBlocks Services
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<ITenantContext, TenantContextService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}