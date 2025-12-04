using MimeCheck.Signatures;

namespace MimeCheck.Detection;

/// <summary>
/// Provides methods for detecting MIME types from file content using magic byte signatures.
/// </summary>
public static class MimeDetector
{
    /// <summary>
    /// Gets or sets the default number of bytes to read for detection.
    /// </summary>
    public static int DefaultReadLimit { get; set; } = 4096;

    /// <summary>
    /// Detects the MIME type from a byte array.
    /// </summary>
    /// <param name="data">The byte array containing file content.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(byte[] data)
    {
        return Detect(data.AsSpan());
    }

    /// <summary>
    /// Detects the MIME type from a byte span.
    /// </summary>
    /// <param name="data">The byte span containing file content.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
            return DetectionResult.Unknown;

        foreach (var signature in SignatureDatabase.Signatures)
        {
            if (MatchesSignature(data, signature))
            {
                // Calculate confidence based on signature specificity
                int confidence = CalculateConfidence(signature, data);
                return DetectionResult.FromSignature(signature, confidence);
            }
        }

        return DetectionResult.Unknown;
    }

    /// <summary>
    /// Detects the MIME type from a stream.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <param name="readLimit">Maximum number of bytes to read. Default is <see cref="DefaultReadLimit"/>.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult Detect(Stream stream, int? readLimit = null)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanRead)
            throw new ArgumentException("Stream must be readable.", nameof(stream));

        int limit = readLimit ?? DefaultReadLimit;
        limit = Math.Max(limit, SignatureDatabase.MaxBytesNeeded);

        // Remember original position if seekable
        long originalPosition = stream.CanSeek ? stream.Position : -1;

        try
        {
            byte[] buffer = new byte[limit];
            int bytesRead = ReadFully(stream, buffer, 0, limit);

            if (bytesRead == 0)
                return DetectionResult.Unknown;

            return Detect(buffer.AsSpan(0, bytesRead));
        }
        finally
        {
            // Restore original position if possible
            if (originalPosition >= 0 && stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }

    /// <summary>
    /// Detects the MIME type from a stream asynchronously.
    /// </summary>
    /// <param name="stream">The stream containing file content.</param>
    /// <param name="readLimit">Maximum number of bytes to read. Default is <see cref="DefaultReadLimit"/>.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detection result.</returns>
    public static async Task<DetectionResult> DetectAsync(Stream stream, int? readLimit = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanRead)
            throw new ArgumentException("Stream must be readable.", nameof(stream));

        int limit = readLimit ?? DefaultReadLimit;
        limit = Math.Max(limit, SignatureDatabase.MaxBytesNeeded);

        // Remember original position if seekable
        long originalPosition = stream.CanSeek ? stream.Position : -1;

        try
        {
            byte[] buffer = new byte[limit];
            int bytesRead = await ReadFullyAsync(stream, buffer, 0, limit, cancellationToken).ConfigureAwait(false);

            if (bytesRead == 0)
                return DetectionResult.Unknown;

            return Detect(buffer.AsSpan(0, bytesRead));
        }
        finally
        {
            // Restore original position if possible
            if (originalPosition >= 0 && stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }

    /// <summary>
    /// Detects the MIME type from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="readLimit">Maximum number of bytes to read. Default is <see cref="DefaultReadLimit"/>.</param>
    /// <returns>The detection result.</returns>
    public static DetectionResult DetectFromFile(string filePath, int? readLimit = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Detect(stream, readLimit);
    }

    /// <summary>
    /// Detects the MIME type from a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="readLimit">Maximum number of bytes to read. Default is <see cref="DefaultReadLimit"/>.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detection result.</returns>
    public static async Task<DetectionResult> DetectFromFileAsync(string filePath, int? readLimit = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await DetectAsync(stream, readLimit, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Checks if the data matches a specific signature.
    /// </summary>
    private static bool MatchesSignature(ReadOnlySpan<byte> data, MimeSignature signature)
    {
        // First check the basic magic bytes
        if (!signature.Matches(data))
            return false;

        // If there are additional checks, verify them
        if (signature.AdditionalChecks != null && signature.AdditionalChecks.Length > 0)
        {
            foreach (var check in signature.AdditionalChecks)
            {
                if (!MatchesAdditionalCheck(data, check))
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the data matches an additional check.
    /// </summary>
    private static bool MatchesAdditionalCheck(ReadOnlySpan<byte> data, AdditionalCheck check)
    {
        if (check.SearchAnywhere)
        {
            // Search for the bytes anywhere in the data (up to the limit)
            int searchLimit = Math.Min(check.SearchLimit, data.Length);
            return IndexOf(data[..searchLimit], check.Bytes) >= 0;
        }
        else
        {
            // Check at specific offset
            if (data.Length < check.Offset + check.Bytes.Length)
                return false;

            var slice = data.Slice(check.Offset, check.Bytes.Length);
            return slice.SequenceEqual(check.Bytes);
        }
    }

    /// <summary>
    /// Finds the index of a byte sequence within a span.
    /// </summary>
    private static int IndexOf(ReadOnlySpan<byte> data, byte[] pattern)
    {
        if (pattern.Length == 0 || data.Length < pattern.Length)
            return -1;

        for (int i = 0; i <= data.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (data[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Calculates a confidence score for a signature match.
    /// </summary>
    private static int CalculateConfidence(MimeSignature signature, ReadOnlySpan<byte> data)
    {
        int confidence = 50; // Base confidence

        // More magic bytes = higher confidence
        confidence += Math.Min(signature.MagicBytes.Length * 5, 30);

        // Additional checks increase confidence
        if (signature.AdditionalChecks != null && signature.AdditionalChecks.Length > 0)
        {
            confidence += signature.AdditionalChecks.Length * 10;
        }

        // Non-zero offset indicates more specific signature
        if (signature.Offset > 0)
        {
            confidence += 5;
        }

        // Higher priority signatures are typically more specific
        if (signature.Priority > 50)
        {
            confidence += 5;
        }

        return Math.Min(confidence, 100);
    }

    /// <summary>
    /// Reads bytes from a stream, handling partial reads.
    /// </summary>
    private static int ReadFully(Stream stream, byte[] buffer, int offset, int count)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            int read = stream.Read(buffer, offset + totalRead, count - totalRead);
            if (read == 0)
                break;
            totalRead += read;
        }
        return totalRead;
    }

    /// <summary>
    /// Reads bytes from a stream asynchronously, handling partial reads.
    /// </summary>
    private static async Task<int> ReadFullyAsync(Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            int read = await stream.ReadAsync(buffer.AsMemory(offset + totalRead, count - totalRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
                break;
            totalRead += read;
        }
        return totalRead;
    }
}

