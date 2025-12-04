namespace MimeCheck.Signatures;

/// <summary>
/// Represents a file signature (magic bytes) used to identify file types.
/// </summary>
public sealed class MimeSignature
{
    /// <summary>
    /// Gets the MIME type associated with this signature.
    /// </summary>
    public required string MimeType { get; init; }

    /// <summary>
    /// Gets the file category.
    /// </summary>
    public required MimeCategory Category { get; init; }

    /// <summary>
    /// Gets the magic bytes that identify this file type.
    /// </summary>
    public required byte[] MagicBytes { get; init; }

    /// <summary>
    /// Gets the offset from the start of the file where the magic bytes should be found.
    /// Default is 0 (start of file).
    /// </summary>
    public int Offset { get; init; }

    /// <summary>
    /// Gets the primary file extension for this MIME type (including the dot).
    /// </summary>
    public required string Extension { get; init; }

    /// <summary>
    /// Gets alternative file extensions for this MIME type.
    /// </summary>
    public string[] AlternativeExtensions { get; init; } = [];

    /// <summary>
    /// Gets a human-readable description of the file type.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the optional mask to apply when comparing bytes.
    /// If null, all bytes must match exactly.
    /// </summary>
    public byte[]? Mask { get; init; }

    /// <summary>
    /// Gets the priority for matching. Higher values are checked first.
    /// Useful for distinguishing between formats with similar signatures (e.g., ZIP-based formats).
    /// </summary>
    public int Priority { get; init; }

    /// <summary>
    /// Gets additional bytes to check for more specific identification.
    /// Used for formats like Office documents that are ZIP-based.
    /// </summary>
    public AdditionalCheck[]? AdditionalChecks { get; init; }

    /// <summary>
    /// Checks if the provided bytes match this signature.
    /// </summary>
    /// <param name="data">The byte data to check.</param>
    /// <returns>True if the data matches this signature; otherwise, false.</returns>
    public bool Matches(ReadOnlySpan<byte> data)
    {
        // Check if we have enough data
        if (data.Length < Offset + MagicBytes.Length)
            return false;

        var slice = data.Slice(Offset, MagicBytes.Length);

        // Check magic bytes with optional mask
        for (int i = 0; i < MagicBytes.Length; i++)
        {
            byte expected = MagicBytes[i];
            byte actual = slice[i];

            if (Mask != null && i < Mask.Length)
            {
                // Apply mask
                if ((actual & Mask[i]) != (expected & Mask[i]))
                    return false;
            }
            else
            {
                // Exact match required
                if (actual != expected)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets all extensions (primary and alternatives) for this signature.
    /// </summary>
    public IEnumerable<string> GetAllExtensions()
    {
        yield return Extension;
        foreach (var ext in AlternativeExtensions)
            yield return ext;
    }
}

/// <summary>
/// Represents an additional check for more specific file type identification.
/// </summary>
public sealed class AdditionalCheck
{
    /// <summary>
    /// Gets the bytes to look for.
    /// </summary>
    public required byte[] Bytes { get; init; }

    /// <summary>
    /// Gets the offset where to look for the bytes.
    /// </summary>
    public int Offset { get; init; }

    /// <summary>
    /// Gets whether to search for the bytes anywhere in the file (up to a limit).
    /// </summary>
    public bool SearchAnywhere { get; init; }

    /// <summary>
    /// Gets the maximum bytes to search when SearchAnywhere is true.
    /// </summary>
    public int SearchLimit { get; init; } = 4096;
}

