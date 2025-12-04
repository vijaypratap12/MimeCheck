using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MimeCheck.AspNetCore.Middleware;
using MimeCheck.AspNetCore.Services;

namespace MimeCheck.AspNetCore;

/// <summary>
/// Extension methods for configuring MimeCheck services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds MimeCheck validation services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMimeValidation(this IServiceCollection services)
    {
        return services.AddMimeValidation(_ => { });
    }

    /// <summary>
    /// Adds MimeCheck validation services to the service collection with configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">An action to configure the options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMimeValidation(this IServiceCollection services, Action<MimeValidationOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);
        services.AddScoped<IMimeValidationService, MimeValidationService>();

        return services;
    }

    /// <summary>
    /// Adds MimeCheck validation services configured for image uploads only.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="maxSizeMB">Maximum file size in megabytes. Default is 10MB.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMimeValidationForImages(this IServiceCollection services, long maxSizeMB = 10)
    {
        return services.AddMimeValidation(options =>
        {
            options.AllowImagesOnly();
            options.WithMaxSizeMB(maxSizeMB);
            options.RequireKnownType = true;
        });
    }

    /// <summary>
    /// Adds MimeCheck validation services configured for document uploads only.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="maxSizeMB">Maximum file size in megabytes. Default is 25MB.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMimeValidationForDocuments(this IServiceCollection services, long maxSizeMB = 25)
    {
        return services.AddMimeValidation(options =>
        {
            options.AllowDocumentsOnly();
            options.WithMaxSizeMB(maxSizeMB);
            options.RequireKnownType = true;
        });
    }

    /// <summary>
    /// Adds MimeCheck validation services that deny executable files.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMimeValidationDenyExecutables(this IServiceCollection services)
    {
        return services.AddMimeValidation(options =>
        {
            options.DenyExecutables();
        });
    }

    /// <summary>
    /// Uses the MimeCheck validation middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseMimeValidation(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UseMiddleware<MimeValidationMiddleware>();
    }
}
