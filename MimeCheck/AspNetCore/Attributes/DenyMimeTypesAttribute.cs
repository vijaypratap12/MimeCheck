using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MimeCheck.Validation;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MimeCheck.AspNetCore.Attributes;

/// <summary>
/// Validates that the uploaded file's MIME type is not one of the denied types.
/// Supports wildcard patterns like "application/*".
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class DenyMimeTypesAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the denied MIME types.
    /// </summary>
    public string[] DeniedTypes { get; }

    /// <summary>
    /// Initializes a new instance with the specified denied MIME types.
    /// </summary>
    /// <param name="deniedTypes">The denied MIME types. Supports wildcards like "application/*".</param>
    public DenyMimeTypesAttribute(params string[] deniedTypes)
    {
        DeniedTypes = deniedTypes ?? throw new ArgumentNullException(nameof(deniedTypes));
        if (deniedTypes.Length == 0)
            throw new ArgumentException("At least one denied type must be specified.", nameof(deniedTypes));

        ErrorMessage = "File type '{0}' is not allowed.";
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
            _ => throw new InvalidOperationException($"DenyMimeTypesAttribute can only be applied to IFormFile or IEnumerable<IFormFile> properties.")
        };

        return result;
    }

    private DataAnnotationsValidationResult? ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
            return DataAnnotationsValidationResult.Success;

        using var stream = file.OpenReadStream();
        var detection = MimeValidator.Detect(stream);

        if (detection.MimeType != null)
        {
            bool isDenied = DeniedTypes.Any(t => MimeValidator.MatchesMimeType(detection.MimeType, t));
            if (isDenied)
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
