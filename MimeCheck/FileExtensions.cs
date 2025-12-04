using MimeCheck.Signatures;

namespace MimeCheck;

/// <summary>
/// Provides utilities for working with file extensions and MIME type mappings.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Gets the MIME type for a file extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The MIME type, or "application/octet-stream" if not found.</returns>
    public static string GetMimeType(string extension)
    {
        return SignatureDatabase.GetMimeTypeForExtension(extension) ?? MimeTypes.OctetStream;
    }

    /// <summary>
    /// Gets the file extension for a MIME type.
    /// </summary>
    /// <param name="mimeType">The MIME type.</param>
    /// <returns>The primary file extension (including dot), or null if not found.</returns>
    public static string? GetExtension(string mimeType)
    {
        var signatures = SignatureDatabase.GetByMimeType(mimeType);
        return signatures.Length > 0 ? signatures[0].Extension : null;
    }

    /// <summary>
    /// Gets all file extensions for a MIME type.
    /// </summary>
    /// <param name="mimeType">The MIME type.</param>
    /// <returns>All file extensions for the MIME type.</returns>
    public static IEnumerable<string> GetExtensions(string mimeType)
    {
        var signatures = SignatureDatabase.GetByMimeType(mimeType);
        return signatures.SelectMany(s => s.GetAllExtensions()).Distinct();
    }

    /// <summary>
    /// Gets the file category for an extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The file category, or Unknown if not found.</returns>
    public static MimeCategory GetCategory(string extension)
    {
        var signatures = SignatureDatabase.GetByExtension(extension);
        return signatures.Length > 0 ? signatures[0].Category : MimeCategory.Unknown;
    }

    /// <summary>
    /// Checks if an extension is supported.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>True if the extension is in the database; otherwise, false.</returns>
    public static bool IsSupported(string extension)
    {
        return SignatureDatabase.GetByExtension(extension).Length > 0;
    }

    /// <summary>
    /// Gets all supported extensions for a category.
    /// </summary>
    /// <param name="category">The file category.</param>
    /// <returns>All extensions for the category.</returns>
    public static IEnumerable<string> GetExtensionsForCategory(MimeCategory category)
    {
        return SignatureDatabase.GetByCategory(category)
            .SelectMany(s => s.GetAllExtensions())
            .Distinct();
    }

    #region Common Extension Groups

    /// <summary>
    /// Common image file extensions.
    /// </summary>
    public static readonly string[] ImageExtensions =
    [
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".tif", ".ico", ".svg", ".heic", ".avif"
    ];

    /// <summary>
    /// Common document file extensions.
    /// </summary>
    public static readonly string[] DocumentExtensions =
    [
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".ods", ".odp", ".rtf"
    ];

    /// <summary>
    /// Common archive file extensions.
    /// </summary>
    public static readonly string[] ArchiveExtensions =
    [
        ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz"
    ];

    /// <summary>
    /// Common audio file extensions.
    /// </summary>
    public static readonly string[] AudioExtensions =
    [
        ".mp3", ".wav", ".flac", ".ogg", ".aac", ".m4a", ".wma"
    ];

    /// <summary>
    /// Common video file extensions.
    /// </summary>
    public static readonly string[] VideoExtensions =
    [
        ".mp4", ".avi", ".mkv", ".mov", ".webm", ".flv", ".wmv", ".mpeg", ".mpg"
    ];

    /// <summary>
    /// Common executable file extensions (potentially dangerous).
    /// </summary>
    public static readonly string[] ExecutableExtensions =
    [
        ".exe", ".dll", ".msi", ".bat", ".cmd", ".ps1", ".vbs", ".js", ".jar", ".apk"
    ];

    #endregion
}

