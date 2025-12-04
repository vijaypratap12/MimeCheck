using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeCheck.Detection;
using MimeCheck.Validation;

namespace MimeCheck.AspNetCore.Services;

/// <summary>
/// Default implementation of <see cref="IMimeValidationService"/>.
/// </summary>
public sealed class MimeValidationService : IMimeValidationService
{
    private readonly MimeValidationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="MimeValidationService"/> class.
    /// </summary>
    public MimeValidationService(IOptions<MimeValidationOptions> options)
    {
        _options = options?.Value ?? new MimeValidationOptions();
    }

    /// <inheritdoc />
    public DetectionResult Detect(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var stream = file.OpenReadStream();
        return MimeValidator.Detect(stream);
    }

    /// <inheritdoc />
    public async Task<DetectionResult> DetectAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        await using var stream = file.OpenReadStream();
        return await MimeValidator.DetectAsync(stream, cancellationToken);
    }

    /// <inheritdoc />
    public ValidationResult Validate(IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var stream = file.OpenReadStream();
        var builder = CreateBuilder(file, stream);
        return builder.Validate();
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        await using var stream = file.OpenReadStream();
        var builder = CreateBuilder(file, stream);
        return await builder.ValidateAsync(cancellationToken);
    }

    /// <inheritdoc />
    public ValidationResult Validate(IFormFile file, params string[] allowedMimeTypes)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var stream = file.OpenReadStream();
        return MimeValidator.FromStream(stream)
            .WithFileName(file.FileName)
            .WithFileSize(file.Length)
            .AllowMimeTypes(allowedMimeTypes)
            .Validate();
    }

    /// <inheritdoc />
    public ValidationResult Validate(IFormFile file, MimeCategory allowedCategories)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var stream = file.OpenReadStream();
        return MimeValidator.FromStream(stream)
            .WithFileName(file.FileName)
            .WithFileSize(file.Length)
            .AllowCategories(allowedCategories)
            .Validate();
    }

    /// <inheritdoc />
    public bool IsValid(IFormFile file)
    {
        return Validate(file).IsValid;
    }

    /// <inheritdoc />
    public bool IsValid(IFormFile file, string mimeType)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var stream = file.OpenReadStream();
        var detection = MimeValidator.Detect(stream);
        return MimeValidator.MatchesMimeType(detection.MimeType, mimeType);
    }

    /// <inheritdoc />
    public bool IsImage(IFormFile file)
    {
        return Detect(file).Category == MimeCategory.Image;
    }

    /// <inheritdoc />
    public bool IsDocument(IFormFile file)
    {
        return Detect(file).Category == MimeCategory.Document;
    }

    /// <inheritdoc />
    public bool IsArchive(IFormFile file)
    {
        return Detect(file).Category == MimeCategory.Archive;
    }

    /// <inheritdoc />
    public bool IsExecutable(IFormFile file)
    {
        return Detect(file).Category == MimeCategory.Executable;
    }

    private MimeValidatorBuilder CreateBuilder(IFormFile file, Stream stream)
    {
        var builder = new MimeValidatorBuilder()
            .WithStream(stream)
            .WithFileName(file.FileName)
            .WithFileSize(file.Length);

        // Apply options
        if (_options.AllowedMimeTypes.Count > 0)
        {
            builder.AllowMimeTypes([.. _options.AllowedMimeTypes]);
        }

        if (_options.DeniedMimeTypes.Count > 0)
        {
            builder.DenyMimeTypes([.. _options.DeniedMimeTypes]);
        }

        if (_options.AllowedCategories != MimeCategory.All)
        {
            builder.AllowCategories(_options.AllowedCategories);
        }

        if (_options.DeniedCategories != MimeCategory.Unknown)
        {
            builder.DenyCategories(_options.DeniedCategories);
        }

        if (_options.MaxFileSizeBytes > 0)
        {
            builder.MaxSize(_options.MaxFileSizeBytes);
        }

        if (_options.MinFileSizeBytes > 0)
        {
            builder.MinSize(_options.MinFileSizeBytes);
        }

        if (_options.RequireKnownType)
        {
            builder.RequireKnownType();
        }

        if (_options.ValidateExtension)
        {
            builder.ValidateExtension();
        }

        return builder;
    }
}
