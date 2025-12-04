using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MimeCheck.Validation;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MimeCheck.AspNetCore.Attributes;

/// <summary>
/// Validates that the uploaded file belongs to one of the allowed categories.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AllowedCategoriesAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the allowed categories.
    /// </summary>
    public MimeCategory AllowedCategories { get; }

    /// <summary>
    /// Gets or sets whether to require a known file type.
    /// Default is true.
    /// </summary>
    public bool RequireKnownType { get; set; } = true;

    /// <summary>
    /// Initializes a new instance with the specified allowed categories.
    /// </summary>
    /// <param name="allowedCategories">The allowed file categories.</param>
    public AllowedCategoriesAttribute(MimeCategory allowedCategories)
    {
        AllowedCategories = allowedCategories;
        ErrorMessage = "File category is not allowed. Allowed categories: {0}";
    }

    /// <summary>
    /// Validates the file's category.
    /// </summary>
    protected override DataAnnotationsValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return DataAnnotationsValidationResult.Success;

        var result = value switch
        {
            IFormFile file => ValidateFile(file),
            IEnumerable<IFormFile> files => ValidateFiles(files),
            _ => throw new InvalidOperationException($"AllowedCategoriesAttribute can only be applied to IFormFile or IEnumerable<IFormFile> properties.")
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

        if (detection.Category != MimeCategory.Unknown)
        {
            if ((detection.Category & AllowedCategories) == 0)
            {
                return new DataAnnotationsValidationResult(string.Format(ErrorMessage ?? "File category '{0}' is not allowed.", detection.Category));
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
