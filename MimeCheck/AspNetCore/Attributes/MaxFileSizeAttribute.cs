using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace MimeCheck.AspNetCore.Attributes;

/// <summary>
/// Validates that the uploaded file does not exceed the maximum size.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MaxFileSizeAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the maximum file size in bytes.
    /// </summary>
    public long MaxSizeBytes { get; }

    /// <summary>
    /// Initializes a new instance with the specified maximum size in bytes.
    /// </summary>
    /// <param name="maxSizeBytes">The maximum file size in bytes.</param>
    public MaxFileSizeAttribute(long maxSizeBytes)
    {
        if (maxSizeBytes <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxSizeBytes), "Max size must be greater than zero.");

        MaxSizeBytes = maxSizeBytes;
        ErrorMessage = "File size exceeds the maximum allowed size of {0}.";
    }

    /// <summary>
    /// Creates an attribute with the maximum size in kilobytes.
    /// </summary>
    public static MaxFileSizeAttribute FromKilobytes(long kilobytes) => new(kilobytes * 1024);

    /// <summary>
    /// Creates an attribute with the maximum size in megabytes.
    /// </summary>
    public static MaxFileSizeAttribute FromMegabytes(long megabytes) => new(megabytes * 1024 * 1024);

    /// <summary>
    /// Validates the file size.
    /// </summary>
    protected override DataAnnotationsValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return DataAnnotationsValidationResult.Success;

        var result = value switch
        {
            IFormFile file => ValidateFile(file),
            IEnumerable<IFormFile> files => ValidateFiles(files),
            _ => throw new InvalidOperationException($"MaxFileSizeAttribute can only be applied to IFormFile or IEnumerable<IFormFile> properties.")
        };

        return result;
    }

    private DataAnnotationsValidationResult? ValidateFile(IFormFile file)
    {
        if (file.Length > MaxSizeBytes)
        {
            return new DataAnnotationsValidationResult(string.Format(ErrorMessage ?? "File size exceeds the maximum allowed size of {0}.", FormatBytes(MaxSizeBytes)));
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

    private static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        int order = 0;
        double size = bytes;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return $"{size:0.##} {sizes[order]}";
    }
}
