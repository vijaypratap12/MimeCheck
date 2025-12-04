namespace MimeCheck.Validation;

/// <summary>
/// Represents an error that occurred during MIME validation.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Gets additional details about the error.
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Returns a string representation of this error.
    /// </summary>
    public override string ToString() => $"[{Code}] {Message}";

    #region Common Error Codes

    /// <summary>Error code for unknown file type.</summary>
    public const string UnknownTypeCode = "UNKNOWN_TYPE";

    /// <summary>Error code for disallowed MIME type.</summary>
    public const string DisallowedTypeCode = "DISALLOWED_TYPE";

    /// <summary>Error code for disallowed category.</summary>
    public const string DisallowedCategoryCode = "DISALLOWED_CATEGORY";

    /// <summary>Error code for file too large.</summary>
    public const string FileTooLargeCode = "FILE_TOO_LARGE";

    /// <summary>Error code for file too small.</summary>
    public const string FileTooSmallCode = "FILE_TOO_SMALL";

    /// <summary>Error code for extension mismatch.</summary>
    public const string ExtensionMismatchCode = "EXTENSION_MISMATCH";

    /// <summary>Error code for empty file.</summary>
    public const string EmptyFileCode = "EMPTY_FILE";

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates an error for unknown file type.
    /// </summary>
    public static ValidationError UnknownType() => new()
    {
        Code = UnknownTypeCode,
        Message = "Unable to detect file type."
    };

    /// <summary>
    /// Creates an error for disallowed MIME type.
    /// </summary>
    public static ValidationError DisallowedType(string mimeType, string[]? allowedTypes = null) => new()
    {
        Code = DisallowedTypeCode,
        Message = $"MIME type '{mimeType}' is not allowed.",
        Details = allowedTypes != null ? $"Allowed types: {string.Join(", ", allowedTypes)}" : null
    };

    /// <summary>
    /// Creates an error for explicitly denied MIME type.
    /// </summary>
    public static ValidationError DeniedType(string mimeType) => new()
    {
        Code = DisallowedTypeCode,
        Message = $"MIME type '{mimeType}' is explicitly denied."
    };

    /// <summary>
    /// Creates an error for disallowed category.
    /// </summary>
    public static ValidationError DisallowedCategory(MimeCategory category, MimeCategory? allowedCategories = null) => new()
    {
        Code = DisallowedCategoryCode,
        Message = $"File category '{category}' is not allowed.",
        Details = allowedCategories.HasValue ? $"Allowed categories: {allowedCategories}" : null
    };

    /// <summary>
    /// Creates an error for file too large.
    /// </summary>
    public static ValidationError FileTooLarge(long actualSize, long maxSize) => new()
    {
        Code = FileTooLargeCode,
        Message = $"File size ({FormatBytes(actualSize)}) exceeds maximum allowed size ({FormatBytes(maxSize)})."
    };

    /// <summary>
    /// Creates an error for file too small.
    /// </summary>
    public static ValidationError FileTooSmall(long actualSize, long minSize) => new()
    {
        Code = FileTooSmallCode,
        Message = $"File size ({FormatBytes(actualSize)}) is below minimum required size ({FormatBytes(minSize)})."
    };

    /// <summary>
    /// Creates an error for extension mismatch.
    /// </summary>
    public static ValidationError ExtensionMismatch(string actualExtension, string expectedExtension, string detectedMimeType) => new()
    {
        Code = ExtensionMismatchCode,
        Message = $"File extension '{actualExtension}' does not match detected type '{detectedMimeType}' (expected '{expectedExtension}')."
    };

    /// <summary>
    /// Creates an error for empty file.
    /// </summary>
    public static ValidationError EmptyFile() => new()
    {
        Code = EmptyFileCode,
        Message = "File is empty."
    };

    private static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        int order = 0;
        double size = bytes;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return $"{size:0.##} {sizes[order]}";
    }

    #endregion
}

