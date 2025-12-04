using MimeCheck.Signatures.Categories;

namespace MimeCheck.Signatures;

/// <summary>
/// Provides access to the database of file signatures for MIME type detection.
/// </summary>
public static class SignatureDatabase
{
    private static readonly Lazy<IReadOnlyList<MimeSignature>> _signatures = new(LoadSignatures);
    private static readonly Lazy<IReadOnlyDictionary<string, MimeSignature[]>> _byMimeType = new(BuildMimeTypeIndex);
    private static readonly Lazy<IReadOnlyDictionary<string, MimeSignature[]>> _byExtension = new(BuildExtensionIndex);
    private static readonly Lazy<IReadOnlyDictionary<MimeCategory, MimeSignature[]>> _byCategory = new(BuildCategoryIndex);

    /// <summary>
    /// Gets all registered file signatures, ordered by priority (highest first).
    /// </summary>
    public static IReadOnlyList<MimeSignature> Signatures => _signatures.Value;

    /// <summary>
    /// Gets signatures indexed by MIME type.
    /// </summary>
    public static IReadOnlyDictionary<string, MimeSignature[]> ByMimeType => _byMimeType.Value;

    /// <summary>
    /// Gets signatures indexed by file extension.
    /// </summary>
    public static IReadOnlyDictionary<string, MimeSignature[]> ByExtension => _byExtension.Value;

    /// <summary>
    /// Gets signatures indexed by category.
    /// </summary>
    public static IReadOnlyDictionary<MimeCategory, MimeSignature[]> ByCategory => _byCategory.Value;

    /// <summary>
    /// Gets the maximum number of bytes needed to identify any file type.
    /// </summary>
    public static int MaxBytesNeeded { get; private set; } = 512;

    private static IReadOnlyList<MimeSignature> LoadSignatures()
    {
        var signatures = new List<MimeSignature>();

        // Load all category signatures
        signatures.AddRange(ImageSignatures.GetAll());
        signatures.AddRange(DocumentSignatures.GetAll());
        signatures.AddRange(ArchiveSignatures.GetAll());
        signatures.AddRange(AudioSignatures.GetAll());
        signatures.AddRange(VideoSignatures.GetAll());
        signatures.AddRange(ExecutableSignatures.GetAll());
        signatures.AddRange(FontSignatures.GetAll());
        signatures.AddRange(OtherSignatures.GetAll());

        // Calculate max bytes needed
        int maxOffset = 0;
        foreach (var sig in signatures)
        {
            int needed = sig.Offset + sig.MagicBytes.Length;
            if (sig.AdditionalChecks != null)
            {
                foreach (var check in sig.AdditionalChecks)
                {
                    if (check.SearchAnywhere)
                    {
                        needed = Math.Max(needed, check.SearchLimit);
                    }
                    else
                    {
                        needed = Math.Max(needed, check.Offset + check.Bytes.Length);
                    }
                }
            }
            maxOffset = Math.Max(maxOffset, needed);
        }
        MaxBytesNeeded = Math.Max(512, maxOffset);

        // Sort by priority (descending) for optimal matching
        return signatures.OrderByDescending(s => s.Priority).ToList().AsReadOnly();
    }

    private static IReadOnlyDictionary<string, MimeSignature[]> BuildMimeTypeIndex()
    {
        return Signatures
            .GroupBy(s => s.MimeType, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToArray(), StringComparer.OrdinalIgnoreCase);
    }

    private static IReadOnlyDictionary<string, MimeSignature[]> BuildExtensionIndex()
    {
        var dict = new Dictionary<string, List<MimeSignature>>(StringComparer.OrdinalIgnoreCase);

        foreach (var sig in Signatures)
        {
            foreach (var ext in sig.GetAllExtensions())
            {
                var normalizedExt = ext.StartsWith('.') ? ext : "." + ext;
                if (!dict.TryGetValue(normalizedExt, out var list))
                {
                    list = [];
                    dict[normalizedExt] = list;
                }
                list.Add(sig);
            }
        }

        return dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray(), StringComparer.OrdinalIgnoreCase);
    }

    private static IReadOnlyDictionary<MimeCategory, MimeSignature[]> BuildCategoryIndex()
    {
        return Signatures
            .GroupBy(s => s.Category)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    /// <summary>
    /// Gets all signatures for a specific category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    /// <returns>An array of signatures matching the category.</returns>
    public static MimeSignature[] GetByCategory(MimeCategory category)
    {
        if (ByCategory.TryGetValue(category, out var signatures))
            return signatures;

        // Handle flags - return all signatures that match any of the flags
        return Signatures.Where(s => (s.Category & category) != 0).ToArray();
    }

    /// <summary>
    /// Gets signatures by MIME type.
    /// </summary>
    /// <param name="mimeType">The MIME type to search for.</param>
    /// <returns>An array of signatures matching the MIME type, or empty array if not found.</returns>
    public static MimeSignature[] GetByMimeType(string mimeType)
    {
        return ByMimeType.TryGetValue(mimeType, out var signatures) ? signatures : [];
    }

    /// <summary>
    /// Gets signatures by file extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>An array of signatures matching the extension, or empty array if not found.</returns>
    public static MimeSignature[] GetByExtension(string extension)
    {
        var normalizedExt = extension.StartsWith('.') ? extension : "." + extension;
        return ByExtension.TryGetValue(normalizedExt, out var signatures) ? signatures : [];
    }

    /// <summary>
    /// Gets the MIME type for a file extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The MIME type, or null if not found.</returns>
    public static string? GetMimeTypeForExtension(string extension)
    {
        var signatures = GetByExtension(extension);
        return signatures.Length > 0 ? signatures[0].MimeType : null;
    }

    /// <summary>
    /// Gets all supported file extensions.
    /// </summary>
    /// <returns>A collection of all supported file extensions.</returns>
    public static IEnumerable<string> GetAllExtensions()
    {
        return ByExtension.Keys;
    }

    /// <summary>
    /// Gets all supported MIME types.
    /// </summary>
    /// <returns>A collection of all supported MIME types.</returns>
    public static IEnumerable<string> GetAllMimeTypes()
    {
        return ByMimeType.Keys;
    }
}

