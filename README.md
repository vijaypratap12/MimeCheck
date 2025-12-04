# MimeCheck

[![NuGet](https://img.shields.io/nuget/v/MimeCheck.svg)](https://www.nuget.org/packages/MimeCheck/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive MIME type validation library for .NET. Detect and validate file types using magic byte signatures with support for 50+ file formats.

## Features

- 🔍 **Magic Byte Detection** - Identify file types by reading actual file headers, not just extensions
- ✅ **Fluent Validation API** - Chain validation rules with an intuitive builder pattern
- 🛡️ **ASP.NET Core Integration** - Validation attributes, middleware, and DI support
- 📁 **Category-Based Filtering** - Filter by Image, Document, Archive, Audio, Video, Executable, Font, etc.
- ⚡ **Async/Await Support** - Full async support for stream and file operations
- 💉 **Dependency Injection** - Ready-to-use services for ASP.NET Core
- 🎯 **50+ File Formats** - Extensive support for common and specialized formats

## Supported File Formats

| Category | Formats |
|----------|---------|
| **Images** | JPEG, PNG, GIF, BMP, WebP, TIFF, ICO, SVG, PSD, RAW |
| **Documents** | PDF, DOCX, XLSX, PPTX, DOC, XLS, PPT, ODT, RTF |
| **Archives** | ZIP, RAR, 7Z, TAR, GZ, BZ2, XZ |
| **Audio** | MP3, WAV, FLAC, OGG, AAC, WMA, AIFF |
| **Video** | MP4, AVI, MKV, MOV, WMV, FLV, WebM |
| **Executables** | EXE, DLL, MSI, ELF, Mach-O |
| **Fonts** | TTF, OTF, WOFF, WOFF2, EOT |
| **Other** | SQLite, XML, JSON, and more |

## Installation

```bash
dotnet add package MimeCheck
```

Or via Package Manager:

```powershell
Install-Package MimeCheck
```

## Quick Start

### Basic MIME Detection

```csharp
using MimeCheck.Validation;
using MimeCheck.Detection;

// Detect from file path
var result = MimeValidator.DetectFromFile("photo.jpg");
Console.WriteLine($"MIME Type: {result.MimeType}");     // image/jpeg
Console.WriteLine($"Category: {result.Category}");      // Image
Console.WriteLine($"Confidence: {result.Confidence}");  // High

// Detect from byte array
byte[] fileBytes = File.ReadAllBytes("document.pdf");
var detection = MimeValidator.Detect(fileBytes);

// Detect from stream
using var stream = File.OpenRead("archive.zip");
var streamResult = await MimeValidator.DetectAsync(stream);
```

### Fluent Validation API

```csharp
using MimeCheck.Validation;

// Validate with chained rules
var validation = MimeValidator.FromFile("upload.pdf")
    .AllowMimeTypes("application/pdf", "application/msword")
    .DenyExecutables()
    .MaxSizeMB(10)
    .ValidateExtension()
    .Validate();

if (validation.IsValid)
{
    Console.WriteLine("File is valid!");
}
else
{
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"Error: {error.Message}");
    }
}

// Category-based validation
var imageValidation = MimeValidator.FromBytes(fileBytes)
    .AllowImages()
    .AllowMimeTypes("image/svg+xml") // Also allow SVG
    .MaxSizeMB(5)
    .Validate();
```

### Quick Validation Methods

```csharp
// Simple type checks
bool isImage = MimeValidator.IsImage("photo.png");
bool isDocument = MimeValidator.IsDocument("report.pdf");
bool isArchive = MimeValidator.IsArchive("backup.zip");
bool isExecutable = MimeValidator.IsExecutable("app.exe");

// Check specific MIME types
bool isPdf = MimeValidator.IsValid("file.pdf", "application/pdf");
bool isJpegOrPng = MimeValidator.IsValid("image.jpg", "image/jpeg", "image/png");
```

## ASP.NET Core Integration

### Setup

```csharp
// Program.cs
using MimeCheck.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add MimeCheck services
builder.Services.AddMimeValidation(options =>
{
    options.DenyExecutables();
    options.WithMaxSizeMB(50);
    options.AllowCategories(MimeCategory.Image, MimeCategory.Document);
});

// Or use preset configurations
builder.Services.AddMimeValidationForImages(); // Only images
builder.Services.AddSecureMimeValidation();    // No executables, 100MB limit
```

### Validation Attributes

```csharp
using MimeCheck.AspNetCore.Attributes;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    // Allow only specific MIME types
    [HttpPost("document")]
    public IActionResult UploadDocument(
        [AllowedMimeTypes("application/pdf", "application/msword")] 
        IFormFile file)
    {
        return Ok(new { file.FileName, file.Length });
    }

    // Allow by category
    [HttpPost("image")]
    public IActionResult UploadImage(
        [AllowedCategories(MimeCategory.Image)]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        IFormFile file)
    {
        return Ok(new { file.FileName });
    }

    // Deny dangerous files
    [HttpPost("safe")]
    public IActionResult UploadSafe(
        [DenyMimeTypes("application/x-msdownload", "application/x-executable")]
        IFormFile file)
    {
        return Ok(new { file.FileName });
    }

    // Full validation with multiple rules
    [HttpPost("secure")]
    public IActionResult UploadSecure(
        [ValidateMimeType(
            AllowedTypes = new[] { "image/jpeg", "image/png", "application/pdf" },
            MaxSizeBytes = 10 * 1024 * 1024,
            ValidateExtension = true)]
        IFormFile file)
    {
        return Ok(new { file.FileName });
    }
}
```

### Using the Validation Service

```csharp
using MimeCheck.AspNetCore.Services;

public class FileService
{
    private readonly IMimeValidationService _mimeValidator;

    public FileService(IMimeValidationService mimeValidator)
    {
        _mimeValidator = mimeValidator;
    }

    public async Task<bool> ProcessUpload(IFormFile file)
    {
        // Detect MIME type
        var detection = await _mimeValidator.DetectAsync(file);
        
        if (!detection.IsDetected)
            return false;

        // Validate with rules
        var validation = await _mimeValidator.ValidateAsync(file);
        
        return validation.IsValid;
    }
}
```

### Middleware (Optional)

```csharp
// Program.cs - Enable automatic validation middleware
builder.Services.AddMimeValidation(options =>
{
    options.EnableMiddleware = true;
    options.IncludePaths.Add("/api/upload");
    options.ExcludePaths.Add("/api/upload/raw");
});

var app = builder.Build();
app.UseMimeValidation(); // Add before UseEndpoints
```

## Advanced Usage

### Custom Validation Logic

```csharp
var result = MimeValidator.FromFile("file.dat")
    .AllowMimeTypes("application/octet-stream")
    .WithCustomValidation(detection =>
    {
        // Add custom validation logic
        if (detection.MimeType == "application/octet-stream")
        {
            // Perform additional checks
            return true;
        }
        return false;
    })
    .Validate();
```

### Working with Streams

```csharp
// Async stream validation
await using var stream = File.OpenRead("large-file.zip");

var result = await MimeValidator.FromStream(stream)
    .AllowArchives()
    .MaxSizeMB(100)
    .ValidateAsync();
```

### Extension Utilities

```csharp
using MimeCheck;

// Get MIME type from extension
string mimeType = FileExtensions.GetMimeType(".pdf"); // application/pdf

// Get extension from MIME type
string extension = FileExtensions.GetExtension("image/jpeg"); // .jpg

// Get category
MimeCategory category = FileExtensions.GetCategory(".docx"); // Document

// Check if extension is supported
bool supported = FileExtensions.IsSupported(".png"); // true
```

## Error Handling

```csharp
var result = MimeValidator.FromFile("suspicious.exe")
    .DenyExecutables()
    .AllowDocuments()
    .Validate();

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"[{error.Code}] {error.Message}");
        // Example output:
        // [MIME_TYPE_DENIED] The MIME type 'application/x-msdownload' is not allowed
        // [CATEGORY_NOT_ALLOWED] File category 'Executable' is not in the allowed list
    }
}
```

## Performance Considerations

- **Minimal Read**: Only reads the bytes needed for detection (typically 8-262 bytes)
- **No Full File Load**: Works with streams without loading entire files into memory
- **Cached Signatures**: Signature database is loaded once and cached
- **Async Support**: Use async methods for I/O-bound operations

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

If you encounter any issues or have questions, please [open an issue](https://github.com/vijaysingh/MimeCheck/issues) on GitHub.

