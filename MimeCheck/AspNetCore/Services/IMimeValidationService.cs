using Microsoft.AspNetCore.Http;
using MimeCheck.Detection;
using MimeCheck.Validation;

namespace MimeCheck.AspNetCore.Services;

/// <summary>
/// Service interface for MIME type validation.
/// </summary>
public interface IMimeValidationService
{
    /// <summary>
    /// Detects the MIME type of an uploaded file.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <returns>The detection result.</returns>
    DetectionResult Detect(IFormFile file);

    /// <summary>
    /// Detects the MIME type of an uploaded file asynchronously.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detection result.</returns>
    Task<DetectionResult> DetectAsync(IFormFile file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates an uploaded file using the configured options.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(IFormFile file);

    /// <summary>
    /// Validates an uploaded file asynchronously using the configured options.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The validation result.</returns>
    Task<ValidationResult> ValidateAsync(IFormFile file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates an uploaded file against specific MIME types.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="allowedMimeTypes">The allowed MIME types.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(IFormFile file, params string[] allowedMimeTypes);

    /// <summary>
    /// Validates an uploaded file against specific categories.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="allowedCategories">The allowed categories.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(IFormFile file, MimeCategory allowedCategories);

    /// <summary>
    /// Checks if a file is valid according to configured options.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    bool IsValid(IFormFile file);

    /// <summary>
    /// Checks if a file is of a specific MIME type.
    /// </summary>
    /// <param name="file">The uploaded file.</param>
    /// <param name="mimeType">The expected MIME type.</param>
    /// <returns>True if the file matches the MIME type; otherwise, false.</returns>
    bool IsValid(IFormFile file, string mimeType);

    /// <summary>
    /// Checks if a file is an image.
    /// </summary>
    bool IsImage(IFormFile file);

    /// <summary>
    /// Checks if a file is a document.
    /// </summary>
    bool IsDocument(IFormFile file);

    /// <summary>
    /// Checks if a file is an archive.
    /// </summary>
    bool IsArchive(IFormFile file);

    /// <summary>
    /// Checks if a file is an executable.
    /// </summary>
    bool IsExecutable(IFormFile file);
}

