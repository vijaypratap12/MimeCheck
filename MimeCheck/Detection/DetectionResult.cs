using MimeCheck.Signatures;

namespace MimeCheck.Detection;

/// <summary>
/// Represents the result of a MIME type detection operation.
/// </summary>
public sealed class DetectionResult
{
    /// <summary>
    /// Gets a value indicating whether a MIME type was successfully detected.
    /// </summary>
    public bool IsDetected { get; init; }

    /// <summary>
    /// Gets the detected MIME type, or null if detection failed.
    /// </summary>
    public string? MimeType { get; init; }

    /// <summary>
    /// Gets the detected file category.
    /// </summary>
    public MimeCategory Category { get; init; }

    /// <summary>
    /// Gets the primary file extension for the detected type (including the dot).
    /// </summary>
    public string? Extension { get; init; }

    /// <summary>
    /// Gets alternative file extensions for the detected type.
    /// </summary>
    public string[] AlternativeExtensions { get; init; } = [];

    /// <summary>
    /// Gets a human-readable description of the detected file type.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the confidence level of the detection (0-100).
    /// Higher values indicate more specific/certain matches.
    /// </summary>
    public int Confidence { get; init; }

    /// <summary>
    /// Gets the signature that matched, if any.
    /// </summary>
    public MimeSignature? MatchedSignature { get; init; }

    /// <summary>
    /// Gets a result representing an unknown/undetected file type.
    /// </summary>
    public static DetectionResult Unknown { get; } = new()
    {
        IsDetected = false,
        MimeType = MimeTypes.OctetStream,
        Category = MimeCategory.Unknown,
        Extension = null,
        Description = "Unknown file type",
        Confidence = 0
    };

    /// <summary>
    /// Creates a successful detection result from a matched signature.
    /// </summary>
    /// <param name="signature">The matched signature.</param>
    /// <param name="confidence">The confidence level (0-100).</param>
    /// <returns>A new detection result.</returns>
    internal static DetectionResult FromSignature(MimeSignature signature, int confidence = 100)
    {
        return new DetectionResult
        {
            IsDetected = true,
            MimeType = signature.MimeType,
            Category = signature.Category,
            Extension = signature.Extension,
            AlternativeExtensions = signature.AlternativeExtensions,
            Description = signature.Description,
            Confidence = confidence,
            MatchedSignature = signature
        };
    }

    /// <summary>
    /// Returns a string representation of this detection result.
    /// </summary>
    public override string ToString()
    {
        if (!IsDetected)
            return "Unknown";

        return $"{MimeType} ({Extension}) - {Description} [{Confidence}%]";
    }
}

