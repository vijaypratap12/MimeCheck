using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MimeCheck.Validation;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MimeCheck.AspNetCore.Attributes;

/// <summary>
/// Comprehensive MIME validation attribute that combines multiple validation rules.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class ValidateMimeTypeAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets the allowed MIME types. Supports wildcards like "image/*".
    /// </summary>
    public string[]? AllowedMimeTypes { get; set; }

    /// <summary>
    /// Gets or sets the denied MIME types.
    /// </summary>
    public string[]? DeniedMimeTypes { get; set; }

    /// <summary>
    /// Gets or sets the allowed categories.
    /// </summary>
    public MimeCategory AllowedCategories { get; set; } = MimeCategory.All;

    /// <summary>
    /// Gets or sets the denied categories.
    /// </summary>
    public MimeCategory DeniedCategories { get; set; } = MimeCategory.Unknown;

    /// <summary>
    /// Gets or sets the maximum file size in bytes.
    /// </summary>
    public long MaxSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the minimum file size in bytes.
    /// </summary>
    public long MinSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets whether to require a known file type.
    /// Default is false.
    /// </summary>
    public bool RequireKnownType { get; set; }

    /// <summary>
    /// Gets or sets whether to validate that the extension matches the detected type.
    /// Default is false.
    /// </summary>
    public bool ValidateExtension { get; set; }

    /// <summary>
    /// Validates the file.
    /// </summary>
    protected override DataAnnotationsValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return DataAnnotationsValidationResult.Success;

        var result = value switch
        {
            IFormFile file => ValidateFile(file),
            IEnumerable<IFormFile> files => ValidateFiles(files),
            _ => throw new InvalidOperationException($"ValidateMimeTypeAttribute can only be applied to IFormFile or IEnumerable<IFormFile> properties.")
        };

        return result;
    }

    private DataAnnotationsValidationResult? ValidateFile(IFormFile file)
    {
        var builder = new MimeValidatorBuilder()
            .WithFileName(file.FileName)
            .WithFileSize(file.Length);

        using var stream = file.OpenReadStream();
        builder.WithStream(stream);

        // Apply rules
        if (AllowedMimeTypes != null && AllowedMimeTypes.Length > 0)
        {
            builder.AllowMimeTypes(AllowedMimeTypes);
        }

        if (DeniedMimeTypes != null && DeniedMimeTypes.Length > 0)
        {
            builder.DenyMimeTypes(DeniedMimeTypes);
        }

        if (AllowedCategories != MimeCategory.All)
        {
            builder.AllowCategories(AllowedCategories);
        }

        if (DeniedCategories != MimeCategory.Unknown)
        {
            builder.DenyCategories(DeniedCategories);
        }

        if (MaxSizeBytes > 0)
        {
            builder.MaxSize(MaxSizeBytes);
        }

        if (MinSizeBytes > 0)
        {
            builder.MinSize(MinSizeBytes);
        }

        if (RequireKnownType)
        {
            builder.RequireKnownType();
        }

        if (ValidateExtension)
        {
            builder.ValidateExtension();
        }

        var validationResult = builder.Validate();

        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.Message));
            return new DataAnnotationsValidationResult(errorMessage);
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
