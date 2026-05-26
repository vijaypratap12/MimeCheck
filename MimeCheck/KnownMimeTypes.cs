namespace MimeCheck;

/// <summary>
/// Registry of MIME types that have no byte signature — plain-text formats,
/// transport types, and structured-syntax variants without distinctive headers.
/// Combined with the signature database (which supplies bytes-detectable types
/// and their aliases), this forms the complete set of MIME strings recognized
/// by <see cref="Validation.MimeValidator.IsKnownMimeType"/>.
/// </summary>
internal static class KnownMimeTypes
{
    /// <summary>
    /// MIME types with no byte signature. Case-insensitive set.
    /// </summary>
    public static readonly HashSet<string> SignatureLess =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Plain-text formats (no distinctive header bytes)
            "text/csv",
            "text/markdown",
            "text/yaml",
            "text/tab-separated-values",
            "text/css",
            "text/javascript",

            // Transport / wrapper types (never stored as files)
            "application/x-www-form-urlencoded",
            "multipart/form-data",
            "multipart/mixed",
            "multipart/alternative",
            "message/rfc822",

            // Structured-syntax variants without distinctive bytes
            "application/ld+json",
            "application/problem+json",
            "application/xhtml+xml",
        };
}
