using MimeCheck.Detection;
using MimeCheck.Signatures;

namespace MimeCheck.Validation;

/// <summary>
/// Provides static methods for MIME type detection and validation.
/// </summary>
public static class MimeValidator
{
    #region Detection Methods

    /// <summary>
    /// Detects the MIME type from a byte array.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(byte[] data) => MimeDetector.Detect(data);

    /// <summary>
    /// Detects the MIME type from a byte span.
    /// </summary>
    /// <param name="data">The byte span containing file content.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(ReadOnlySpan<byte> data) => MimeDetector.Detect(data);

    /// <summary>
    /// Detects the MIME type from a stream.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(Stream stream) => MimeDetector.Detect(stream);

    /// <summary>
    /// Detects the MIME type from a stream asynchronously.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detection result.</returns>
    public static Task<DetectionResult> DetectAsync(Stream stream, CancellationToken cancellationToken = default)
        => MimeDetector.DetectAsync(stream, cancellationToken: cancellationToken);

    /// <summary>
    /// Detects the MIME type from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult DetectFromFile(string filePath) => MimeDetector.DetectFromFile(filePath);

    /// <summary>
    /// Detects the MIME type from a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detection result.</returns>
    public static Task<DetectionResult> DetectFromFileAsync(string filePath, CancellationToken cancellationToken = default)
        => MimeDetector.DetectFromFileAsync(filePath, cancellationToken: cancellationToken);

    /// <summary>
    /// Gets the MIME type string from a byte array.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <returns>The MIME type string, or "application/octet-stream" if unknown.</returns>
    public static string GetMimeType(byte[] data) => Detect(data).MimeType ?? MimeTypes.OctetStream;

    /// <summary>
    /// Gets the MIME type string from a stream.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <returns>The MIME type string, or "application/octet-stream" if unknown.</returns>
    public static string GetMimeType(Stream stream) => Detect(stream).MimeType ?? MimeTypes.OctetStream;

    /// <summary>
    /// Gets the MIME type string from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>The MIME type string, or "application/octet-stream" if unknown.</returns>
    public static string GetMimeType(string filePath) => DetectFromFile(filePath).MimeType ?? MimeTypes.OctetStream;

    #endregion

    #region Simple Validation Methods

    /// <summary>
    /// Checks if the data matches a specific MIME type.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <param name="expectedMimeType">The expected MIME type.</param>
    /// <returns>True if the detected MIME type matches; otherwise, false.</returns>
    public static bool IsValid(byte[] data, string expectedMimeType)
    {
        var result = Detect(data);
        return MatchesMimeType(result.MimeType, expectedMimeType);
    }

    /// <summary>
    /// Checks if the stream content matches a specific MIME type.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <param name="expectedMimeType">The expected MIME type.</param>
    /// <returns>True if the detected MIME type matches; otherwise, false.</returns>
    public static bool IsValid(Stream stream, string expectedMimeType)
    {
        var result = Detect(stream);
        return MatchesMimeType(result.MimeType, expectedMimeType);
    }

    /// <summary>
    /// Checks if the data matches any of the specified MIME types.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <param name="allowedMimeTypes">The allowed MIME types.</param>
    /// <returns>True if the detected MIME type matches any allowed type; otherwise, false.</returns>
    public static bool IsValid(byte[] data, params string[] allowedMimeTypes)
    {
        var result = Detect(data);
        return allowedMimeTypes.Any(t => MatchesMimeType(result.MimeType, t));
    }

    /// <summary>
    /// Checks if the stream content matches any of the specified MIME types.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <param name="allowedMimeTypes">The allowed MIME types.</param>
    /// <returns>True if the detected MIME type matches any allowed type; otherwise, false.</returns>
    public static bool IsValid(Stream stream, params string[] allowedMimeTypes)
    {
        var result = Detect(stream);
        return allowedMimeTypes.Any(t => MatchesMimeType(result.MimeType, t));
    }

    #endregion

    #region Category Validation Methods

    /// <summary>
    /// Checks if the data is an image.
    /// </summary>
    public static bool IsImage(byte[] data) => Detect(data).Category == MimeCategory.Image;

    /// <summary>
    /// Checks if the stream content is an image.
    /// </summary>
    public static bool IsImage(Stream stream) => Detect(stream).Category == MimeCategory.Image;

    /// <summary>
    /// Checks if the data is a document.
    /// </summary>
    public static bool IsDocument(byte[] data) => Detect(data).Category == MimeCategory.Document;

    /// <summary>
    /// Checks if the stream content is a document.
    /// </summary>
    public static bool IsDocument(Stream stream) => Detect(stream).Category == MimeCategory.Document;

    /// <summary>
    /// Checks if the data is an archive.
    /// </summary>
    public static bool IsArchive(byte[] data) => Detect(data).Category == MimeCategory.Archive;

    /// <summary>
    /// Checks if the stream content is an archive.
    /// </summary>
    public static bool IsArchive(Stream stream) => Detect(stream).Category == MimeCategory.Archive;

    /// <summary>
    /// Checks if the data is audio.
    /// </summary>
    public static bool IsAudio(byte[] data) => Detect(data).Category == MimeCategory.Audio;

    /// <summary>
    /// Checks if the stream content is audio.
    /// </summary>
    public static bool IsAudio(Stream stream) => Detect(stream).Category == MimeCategory.Audio;

    /// <summary>
    /// Checks if the data is video.
    /// </summary>
    public static bool IsVideo(byte[] data) => Detect(data).Category == MimeCategory.Video;

    /// <summary>
    /// Checks if the stream content is video.
    /// </summary>
    public static bool IsVideo(Stream stream) => Detect(stream).Category == MimeCategory.Video;

    /// <summary>
    /// Checks if the data is an executable.
    /// </summary>
    public static bool IsExecutable(byte[] data) => Detect(data).Category == MimeCategory.Executable;

    /// <summary>
    /// Checks if the stream content is an executable.
    /// </summary>
    public static bool IsExecutable(Stream stream) => Detect(stream).Category == MimeCategory.Executable;

    /// <summary>
    /// Checks if the data belongs to the specified category.
    /// </summary>
    public static bool IsCategory(byte[] data, MimeCategory category)
    {
        var detected = Detect(data).Category;
        return (detected & category) != 0;
    }

    /// <summary>
    /// Checks if the stream content belongs to the specified category.
    /// </summary>
    public static bool IsCategory(Stream stream, MimeCategory category)
    {
        var detected = Detect(stream).Category;
        return (detected & category) != 0;
    }

    #endregion

    #region Fluent API Entry Points

    /// <summary>
    /// Creates a validation builder from a byte array.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <returns>A validation builder.</returns>
    public static MimeValidatorBuilder FromBytes(byte[] data)
        => new MimeValidatorBuilder().WithData(data);

    /// <summary>
    /// Creates a validation builder from a stream.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <returns>A validation builder.</returns>
    public static MimeValidatorBuilder FromStream(Stream stream)
        => new MimeValidatorBuilder().WithStream(stream);

    /// <summary>
    /// Creates a validation builder from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>A validation builder.</returns>
    public static MimeValidatorBuilder FromFile(string filePath)
        => new MimeValidatorBuilder().WithFile(filePath);

    #endregion

    #region Helper Methods

    /// <summary>
    /// Checks if a MIME type matches a pattern (supports wildcards like "image/*").
    /// </summary>
    internal static bool MatchesMimeType(string? actual, string pattern)
    {
        if (string.IsNullOrEmpty(actual))
            return false;

        if (pattern.Equals(actual, StringComparison.OrdinalIgnoreCase))
            return true;

        // Support wildcard patterns like "image/*"
        if (pattern.EndsWith("/*"))
        {
            var prefix = pattern[..^2];
            return actual.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    /// <summary>
    /// Gets the MIME type for a file extension from the signature database.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The MIME type, or null if not found.</returns>
    public static string? GetMimeTypeForExtension(string extension)
        => SignatureDatabase.GetMimeTypeForExtension(extension);

    /// <summary>
    /// Gets all supported file extensions.
    /// </summary>
    public static IEnumerable<string> GetSupportedExtensions()
        => SignatureDatabase.GetAllExtensions();

    /// <summary>
    /// Gets all supported MIME types.
    /// </summary>
    public static IEnumerable<string> GetSupportedMimeTypes()
        => SignatureDatabase.GetAllMimeTypes();

    #endregion
}

