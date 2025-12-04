using MimeCheck.Detection;

namespace MimeCheck.Validation;

/// <summary>
/// Represents the result of a MIME validation operation.
/// </summary>
public sealed class ValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation passed.
    /// </summary>
    public bool IsValid => Errors.Count == 0;

    /// <summary>
    /// Gets the detection result from the validation.
    /// </summary>
    public DetectionResult Detection { get; init; } = DetectionResult.Unknown;

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    public IReadOnlyList<ValidationError> Errors { get; init; } = [];

    /// <summary>
    /// Gets the file size in bytes, if available.
    /// </summary>
    public long? FileSize { get; init; }

    /// <summary>
    /// Gets the original file name, if available.
    /// </summary>
    public string? FileName { get; init; }

    /// <summary>
    /// Gets the original file extension, if available.
    /// </summary>
    public string? FileExtension { get; init; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success(DetectionResult detection, long? fileSize = null, string? fileName = null) => new()
    {
        Detection = detection,
        FileSize = fileSize,
        FileName = fileName,
        FileExtension = fileName != null ? Path.GetExtension(fileName) : null,
        Errors = []
    };

    /// <summary>
    /// Creates a failed validation result.
    /// </summary>
    public static ValidationResult Failure(DetectionResult detection, params ValidationError[] errors) => new()
    {
        Detection = detection,
        Errors = errors
    };

    /// <summary>
    /// Creates a failed validation result with multiple errors.
    /// </summary>
    public static ValidationResult Failure(DetectionResult detection, IEnumerable<ValidationError> errors, long? fileSize = null, string? fileName = null) => new()
    {
        Detection = detection,
        FileSize = fileSize,
        FileName = fileName,
        FileExtension = fileName != null ? Path.GetExtension(fileName) : null,
        Errors = errors.ToList().AsReadOnly()
    };

    /// <summary>
    /// Returns a string representation of this validation result.
    /// </summary>
    public override string ToString()
    {
        if (IsValid)
            return $"Valid: {Detection}";

        return $"Invalid: {string.Join("; ", Errors.Select(e => e.Message))}";
    }
}

