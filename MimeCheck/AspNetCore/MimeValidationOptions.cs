namespace MimeCheck.AspNetCore;

/// <summary>
/// Configuration options for MIME validation in ASP.NET Core.
/// </summary>
public sealed class MimeValidationOptions
{
    /// <summary>
    /// Gets or sets the allowed MIME types for file uploads.
    /// Supports wildcards like "image/*".
    /// If empty, all types are allowed (unless denied).
    /// </summary>
    public List<string> AllowedMimeTypes { get; set; } = [];

    /// <summary>
    /// Gets or sets the denied MIME types for file uploads.
    /// </summary>
    public List<string> DeniedMimeTypes { get; set; } = [];

    /// <summary>
    /// Gets or sets the allowed file categories.
    /// Default is All.
    /// </summary>
    public MimeCategory AllowedCategories { get; set; } = MimeCategory.All;

    /// <summary>
    /// Gets or sets the denied file categories.
    /// Default is Unknown (no categories denied).
    /// </summary>
    public MimeCategory DeniedCategories { get; set; } = MimeCategory.Unknown;

    /// <summary>
    /// Gets or sets the maximum file size in bytes.
    /// Default is 0 (no limit).
    /// </summary>
    public long MaxFileSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the minimum file size in bytes.
    /// Default is 0 (no minimum).
    /// </summary>
    public long MinFileSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets whether to require a known file type.
    /// Default is false.
    /// </summary>
    public bool RequireKnownType { get; set; }

    /// <summary>
    /// Gets or sets whether to validate that file extensions match detected types.
    /// Default is false.
    /// </summary>
    public bool ValidateExtension { get; set; }

    /// <summary>
    /// Gets or sets whether to enable the validation middleware.
    /// Default is true.
    /// </summary>
    public bool EnableMiddleware { get; set; } = true;

    /// <summary>
    /// Gets or sets the paths to apply validation to.
    /// If empty, validation applies to all paths with file uploads.
    /// </summary>
    public List<string> IncludePaths { get; set; } = [];

    /// <summary>
    /// Gets or sets the paths to exclude from validation.
    /// </summary>
    public List<string> ExcludePaths { get; set; } = [];

    /// <summary>
    /// Configures options to allow only images.
    /// </summary>
    public MimeValidationOptions AllowImagesOnly()
    {
        AllowedCategories = MimeCategory.Image;
        return this;
    }

    /// <summary>
    /// Configures options to allow only documents.
    /// </summary>
    public MimeValidationOptions AllowDocumentsOnly()
    {
        AllowedCategories = MimeCategory.Document;
        return this;
    }

    /// <summary>
    /// Configures options to deny executables.
    /// </summary>
    public MimeValidationOptions DenyExecutables()
    {
        DeniedCategories |= MimeCategory.Executable;
        return this;
    }

    /// <summary>
    /// Sets the maximum file size in megabytes.
    /// </summary>
    public MimeValidationOptions WithMaxSizeMB(long megabytes)
    {
        MaxFileSizeBytes = megabytes * 1024 * 1024;
        return this;
    }
}

