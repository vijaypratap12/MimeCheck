using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeCheck.AspNetCore.Services;

namespace MimeCheck.AspNetCore.Middleware;

/// <summary>
/// Middleware for validating MIME types of uploaded files.
/// </summary>
public class MimeValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MimeValidationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="MimeValidationMiddleware"/> class.
    /// </summary>
    public MimeValidationMiddleware(RequestDelegate next, IOptions<MimeValidationOptions> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options?.Value ?? new MimeValidationOptions();
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IMimeValidationService validationService)
    {
        if (!_options.EnableMiddleware)
        {
            await _next(context);
            return;
        }

        // Check if this path should be processed
        if (!ShouldProcessPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Check if request has files
        if (!context.Request.HasFormContentType)
        {
            await _next(context);
            return;
        }

        try
        {
            var form = await context.Request.ReadFormAsync();

            foreach (var file in form.Files)
            {
                if (file.Length == 0)
                    continue;

                var result = await validationService.ValidateAsync(file, context.RequestAborted);

                if (!result.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var errors = result.Errors.Select(e => new { e.Code, e.Message, e.Details });
                    var response = new
                    {
                        success = false,
                        fileName = file.FileName,
                        errors
                    };

                    await context.Response.WriteAsJsonAsync(response, context.RequestAborted);
                    return;
                }
            }
        }
        catch (InvalidOperationException)
        {
            // Request doesn't have form content, continue
        }

        await _next(context);
    }

    private bool ShouldProcessPath(PathString path)
    {
        // Check exclude paths first
        foreach (var excludePath in _options.ExcludePaths)
        {
            if (path.StartsWithSegments(excludePath, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        // If include paths are specified, check them
        if (_options.IncludePaths.Count > 0)
        {
            foreach (var includePath in _options.IncludePaths)
            {
                if (path.StartsWithSegments(includePath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        // No include paths specified, process all paths
        return true;
    }
}
