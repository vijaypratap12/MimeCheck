using Microsoft.AspNetCore.Mvc;
using MimeCheck.AspNetCore.Attributes;
using MimeCheck.AspNetCore.Services;
using MimeCheck.Validation;

namespace MimeCheck.Controllers;

/// <summary>
/// Sample controller demonstrating MimeCheck file upload validation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IMimeValidationService _validationService;

    public FileUploadController(IMimeValidationService validationService)
    {
        _validationService = validationService;
    }

    /// <summary>
    /// Upload any file and detect its MIME type.
    /// </summary>
    [HttpPost("detect")]
    public async Task<IActionResult> DetectMimeType(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        var result = await _validationService.DetectAsync(file);

        return Ok(new
        {
            fileName = file.FileName,
            fileSize = file.Length,
            detected = result.IsDetected,
            mimeType = result.MimeType,
            category = result.Category.ToString(),
            extension = result.Extension,
            alternativeExtensions = result.AlternativeExtensions,
            description = result.Description,
            confidence = result.Confidence
        });
    }

    /// <summary>
    /// Upload an image file with validation using attribute.
    /// </summary>
    [HttpPost("image")]
    public IActionResult UploadImage(
        [AllowedMimeTypes("image/jpeg", "image/png", "image/gif", "image/webp")]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        var detection = _validationService.Detect(file);

        return Ok(new
        {
            message = "Image uploaded successfully",
            fileName = file.FileName,
            mimeType = detection.MimeType,
            category = detection.Category.ToString()
        });
    }

    /// <summary>
    /// Upload a document file with validation using attribute.
    /// </summary>
    [HttpPost("document")]
    public IActionResult UploadDocument(
        [AllowedCategories(MimeCategory.Document)]
        [MaxFileSize(25 * 1024 * 1024)] // 25MB
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        var detection = _validationService.Detect(file);

        return Ok(new
        {
            message = "Document uploaded successfully",
            fileName = file.FileName,
            mimeType = detection.MimeType,
            category = detection.Category.ToString()
        });
    }

    /// <summary>
    /// Upload any file except executables using attribute.
    /// </summary>
    [HttpPost("safe")]
    public IActionResult UploadSafeFile(
        [DenyMimeTypes("application/x-msdownload", "application/x-executable", "application/x-mach-binary")]
        [ValidateMimeType(DeniedCategories = MimeCategory.Executable, MaxSizeBytes = 50 * 1024 * 1024)]
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        var detection = _validationService.Detect(file);

        return Ok(new
        {
            message = "File uploaded successfully",
            fileName = file.FileName,
            mimeType = detection.MimeType,
            category = detection.Category.ToString()
        });
    }

    /// <summary>
    /// Upload a file with manual validation using the service.
    /// </summary>
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateAndUpload(IFormFile file, [FromQuery] string? allowedTypes)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        ValidationResult result;

        if (!string.IsNullOrEmpty(allowedTypes))
        {
            var types = allowedTypes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            result = _validationService.Validate(file, types);
        }
        else
        {
            result = await _validationService.ValidateAsync(file);
        }

        if (!result.IsValid)
        {
            return BadRequest(new
            {
                valid = false,
                errors = result.Errors.Select(e => new { e.Code, e.Message, e.Details })
            });
        }

        return Ok(new
        {
            valid = true,
            fileName = file.FileName,
            mimeType = result.Detection.MimeType,
            category = result.Detection.Category.ToString(),
            confidence = result.Detection.Confidence
        });
    }

    /// <summary>
    /// Upload a file using the fluent validation API.
    /// </summary>
    [HttpPost("fluent")]
    public async Task<IActionResult> FluentValidation(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        await using var stream = file.OpenReadStream();

        var result = await MimeValidator.FromStream(stream)
            .WithFileName(file.FileName)
            .WithFileSize(file.Length)
            .AllowCategories(MimeCategory.Image | MimeCategory.Document)
            .DenyExecutables()
            .MaxSizeMB(10)
            .RequireKnownType()
            .ValidateExtension()
            .ValidateAsync();

        if (!result.IsValid)
        {
            return BadRequest(new
            {
                valid = false,
                errors = result.Errors.Select(e => new { e.Code, e.Message })
            });
        }

        return Ok(new
        {
            valid = true,
            fileName = file.FileName,
            mimeType = result.Detection.MimeType,
            category = result.Detection.Category.ToString()
        });
    }

    /// <summary>
    /// Get list of supported MIME types.
    /// </summary>
    [HttpGet("supported-types")]
    public IActionResult GetSupportedTypes()
    {
        return Ok(new
        {
            mimeTypes = MimeValidator.GetSupportedMimeTypes().OrderBy(t => t),
            extensions = MimeValidator.GetSupportedExtensions().OrderBy(e => e)
        });
    }

    /// <summary>
    /// Get MIME type for a file extension.
    /// </summary>
    [HttpGet("mime-type")]
    public IActionResult GetMimeType([FromQuery] string extension)
    {
        if (string.IsNullOrEmpty(extension))
            return BadRequest(new { error = "Extension is required" });

        var mimeType = FileExtensions.GetMimeType(extension);
        var category = FileExtensions.GetCategory(extension);

        return Ok(new
        {
            extension,
            mimeType,
            category = category.ToString(),
            isSupported = FileExtensions.IsSupported(extension)
        });
    }
}
