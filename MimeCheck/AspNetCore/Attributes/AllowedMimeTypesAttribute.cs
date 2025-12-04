using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MimeCheck.Validation;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MimeCheck.AspNetCore.Attributes;

/// <summary>
/// Validates that the uploaded file's MIME type matches one of the allowed types.
/// Supports wildcard patterns like "image/*".
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AllowedMimeTypesAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the allowed MIME types.
    /// </summary>
    public string[] AllowedTypes { get; }

    /// <summary>
    /// Gets or sets whether to require a known file type.
    /// Default is true.
    /// </summary>
    public bool RequireKnownType { get; set; } = true;

    /// <summary>
    /// Initializes a new instance with the specified allowed MIME types.
    /// </summary>
    /// <param name="allowedTypes">The allowed MIME types. Supports wildcards like "image/*".</param>
    public AllowedMimeTypesAttribute(params string[] allowedTypes)
    {
        AllowedTypes = allowedTypes ?? throw new ArgumentNullException(nameof(allowedTypes));
        if (allowedTypes.Length == 0)
            throw new ArgumentException("At least one allowed type must be specified.", nameof(allowedTypes));

        ErrorMessage = "File type is not allowed. Allowed types: {0}";
    }

    /// <summary>
    /// Validates the file's MIME type.
    /// </summary>
    protected override DataAnnotationsValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return DataAnnotationsValidationResult.Success;

        var result = value switch
        {
            IFormFile file => ValidateFile(file),
            IEnumerable<IFormFile> files => ValidateFiles(files),
            _ => throw new InvalidOperationException($"AllowedMimeTypesAttribute can only be applied to IFormFile or IEnumerable<IFormFile> properties.")
        };

        return result;
    }

    private DataAnnotationsValidationResult? ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
            return new DataAnnotationsValidationResult("File is empty.");

        using var stream = file.OpenReadStream();
        var detection = MimeValidator.Detect(stream);

        if (RequireKnownType && !detection.IsDetected)
        {
            return new DataAnnotationsValidationResult("Unable to detect file type.");
        }

        if (detection.MimeType != null)
        {
            bool isAllowed = AllowedTypes.Any(t => MimeValidator.MatchesMimeType(detection.MimeType, t));
            if (!isAllowed)
            {
                return new DataAnnotationsValidationResult(string.Format(ErrorMessage ?? "File type '{0}' is not allowed.", detection.MimeType));
            }
        }

        return DataAnnotationsValidationResult.Success;
    }

    private DataAnnotationsValidationResult? ValidateFiles(IEnumerable<IFormFile> files)
    {
        foreach (var file in files)
        {
            var result = ValidateFile(file);
            if (result != DataAnnotationsValidationResult.Success)
                return result;
        }
        return DataAnnotationsValidationResult.Success;
    }
}
