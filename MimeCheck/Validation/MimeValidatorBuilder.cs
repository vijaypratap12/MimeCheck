using MimeCheck.Detection;

namespace MimeCheck.Validation;

/// <summary>
/// Provides a fluent interface for building MIME validation rules.
/// </summary>
public sealed class MimeValidatorBuilder
{
    private byte[]? _data;
    private Stream? _stream;
    private string? _filePath;
    private string? _fileName;
    private long? _fileSize;

    private readonly List<string> _allowedMimeTypes = [];
    private readonly List<string> _deniedMimeTypes = [];
    private MimeCategory _allowedCategories = MimeCategory.All;
    private MimeCategory _deniedCategories = MimeCategory.Unknown;
    private long? _maxSize;
    private long? _minSize;
    private bool _requireKnownType = false;
    private bool _validateExtension = false;

    #region Input Methods

    /// <summary>
    /// Sets the byte array to validate.
    /// </summary>
    public MimeValidatorBuilder WithData(byte[] data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _fileSize = data.Length;
        return this;
    }

    /// <summary>
    /// Sets the stream to validate.
    /// </summary>
    public MimeValidatorBuilder WithStream(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        if (stream.CanSeek)
        {
            _fileSize = stream.Length;
        }
        return this;
    }

    /// <summary>
    /// Sets the file path to validate.
    /// </summary>
    public MimeValidatorBuilder WithFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
        _fileName = Path.GetFileName(filePath);
        if (File.Exists(filePath))
        {
            _fileSize = new FileInfo(filePath).Length;
        }
        return this;
    }

    /// <summary>
    /// Sets the original file name (for extension validation).
    /// </summary>
    public MimeValidatorBuilder WithFileName(string fileName)
    {
        _fileName = fileName;
        return this;
    }

    /// <summary>
    /// Sets the file size (if not determinable from input).
    /// </summary>
    public MimeValidatorBuilder WithFileSize(long size)
    {
        _fileSize = size;
        return this;
    }

    #endregion

    #region Allowed Types

    /// <summary>
    /// Allows only the specified MIME types. Supports wildcards like "image/*".
    /// </summary>
    public MimeValidatorBuilder AllowMimeTypes(params string[] mimeTypes)
    {
        _allowedMimeTypes.AddRange(mimeTypes);
        return this;
    }

    /// <summary>
    /// Allows only files in the specified categories.
    /// </summary>
    public MimeValidatorBuilder AllowCategories(MimeCategory categories)
    {
        _allowedCategories = categories;
        return this;
    }

    /// <summary>
    /// Allows only image files.
    /// </summary>
    public MimeValidatorBuilder AllowImages() => AllowCategories(MimeCategory.Image);

    /// <summary>
    /// Allows only document files.
    /// </summary>
    public MimeValidatorBuilder AllowDocuments() => AllowCategories(MimeCategory.Document);

    /// <summary>
    /// Allows only archive files.
    /// </summary>
    public MimeValidatorBuilder AllowArchives() => AllowCategories(MimeCategory.Archive);

    /// <summary>
    /// Allows only audio files.
    /// </summary>
    public MimeValidatorBuilder AllowAudio() => AllowCategories(MimeCategory.Audio);

    /// <summary>
    /// Allows only video files.
    /// </summary>
    public MimeValidatorBuilder AllowVideo() => AllowCategories(MimeCategory.Video);

    /// <summary>
    /// Allows only media files (images, audio, video).
    /// </summary>
    public MimeValidatorBuilder AllowMedia() => AllowCategories(MimeCategory.Media);

    /// <summary>
    /// Shorthand for allowing common image types.
    /// </summary>
    public MimeValidatorBuilder MustBeImage() => AllowImages();

    /// <summary>
    /// Shorthand for allowing common document types.
    /// </summary>
    public MimeValidatorBuilder MustBeDocument() => AllowDocuments();

    /// <summary>
    /// Requires the file to match a specific MIME type.
    /// </summary>
    public MimeValidatorBuilder MustBe(string mimeType) => AllowMimeTypes(mimeType);

    /// <summary>
    /// Requires the file to match a specific category.
    /// </summary>
    public MimeValidatorBuilder MustBe(MimeCategory category) => AllowCategories(category);

    #endregion

    #region Denied Types

    /// <summary>
    /// Denies the specified MIME types.
    /// </summary>
    public MimeValidatorBuilder DenyMimeTypes(params string[] mimeTypes)
    {
        _deniedMimeTypes.AddRange(mimeTypes);
        return this;
    }

    /// <summary>
    /// Denies files in the specified categories.
    /// </summary>
    public MimeValidatorBuilder DenyCategories(MimeCategory categories)
    {
        _deniedCategories |= categories;
        return this;
    }

    /// <summary>
    /// Denies executable files.
    /// </summary>
    public MimeValidatorBuilder DenyExecutables() => DenyCategories(MimeCategory.Executable);

    /// <summary>
    /// Shorthand for denying a specific MIME type.
    /// </summary>
    public MimeValidatorBuilder MustNotBe(string mimeType) => DenyMimeTypes(mimeType);

    /// <summary>
    /// Shorthand for denying a specific category.
    /// </summary>
    public MimeValidatorBuilder MustNotBe(MimeCategory category) => DenyCategories(category);

    #endregion

    #region Size Constraints

    /// <summary>
    /// Sets the maximum allowed file size in bytes.
    /// </summary>
    public MimeValidatorBuilder MaxSize(long bytes)
    {
        _maxSize = bytes;
        return this;
    }

    /// <summary>
    /// Sets the maximum allowed file size in kilobytes.
    /// </summary>
    public MimeValidatorBuilder MaxSizeKB(long kilobytes) => MaxSize(kilobytes * 1024);

    /// <summary>
    /// Sets the maximum allowed file size in megabytes.
    /// </summary>
    public MimeValidatorBuilder MaxSizeMB(long megabytes) => MaxSize(megabytes * 1024 * 1024);

    /// <summary>
    /// Sets the minimum required file size in bytes.
    /// </summary>
    public MimeValidatorBuilder MinSize(long bytes)
    {
        _minSize = bytes;
        return this;
    }

    #endregion

    #region Additional Validation

    /// <summary>
    /// Requires the file type to be recognized.
    /// </summary>
    public MimeValidatorBuilder RequireKnownType()
    {
        _requireKnownType = true;
        return this;
    }

    /// <summary>
    /// Validates that the file extension matches the detected MIME type.
    /// </summary>
    public MimeValidatorBuilder ValidateExtension()
    {
        _validateExtension = true;
        return this;
    }

    #endregion

    #region Validation Execution

    /// <summary>
    /// Executes the validation and returns the result.
    /// </summary>
    public ValidationResult Validate()
    {
        var errors = new List<ValidationError>();

        // Detect MIME type
        DetectionResult detection;
        if (_data != null)
        {
            detection = MimeDetector.Detect(_data);
        }
        else if (_stream != null)
        {
            detection = MimeDetector.Detect(_stream);
        }
        else if (_filePath != null)
        {
            detection = MimeDetector.DetectFromFile(_filePath);
        }
        else
        {
            throw new InvalidOperationException("No input data provided. Use WithData(), WithStream(), or WithFile().");
        }

        return ValidateWithDetection(detection, errors);
    }

    /// <summary>
    /// Executes the validation asynchronously and returns the result.
    /// </summary>
    public async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
    {
        var errors = new List<ValidationError>();

        // Detect MIME type
        DetectionResult detection;
        if (_data != null)
        {
            detection = MimeDetector.Detect(_data);
        }
        else if (_stream != null)
        {
            detection = await MimeDetector.DetectAsync(_stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else if (_filePath != null)
        {
            detection = await MimeDetector.DetectFromFileAsync(_filePath, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException("No input data provided. Use WithData(), WithStream(), or WithFile().");
        }

        return ValidateWithDetection(detection, errors);
    }

    private ValidationResult ValidateWithDetection(DetectionResult detection, List<ValidationError> errors)
    {
        // Check for empty file
        if (_fileSize == 0)
        {
            errors.Add(ValidationError.EmptyFile());
            return ValidationResult.Failure(detection, errors, _fileSize, _fileName);
        }

        // Check if type is known
        if (_requireKnownType && !detection.IsDetected)
        {
            errors.Add(ValidationError.UnknownType());
        }

        // Check allowed MIME types
        if (_allowedMimeTypes.Count > 0 && detection.MimeType != null)
        {
            bool isAllowed = _allowedMimeTypes.Any(t => MimeValidator.MatchesMimeType(detection.MimeType, t));
            if (!isAllowed)
            {
                errors.Add(ValidationError.DisallowedType(detection.MimeType, [.. _allowedMimeTypes]));
            }
        }

        // Check denied MIME types
        if (_deniedMimeTypes.Count > 0 && detection.MimeType != null)
        {
            bool isDenied = _deniedMimeTypes.Any(t => MimeValidator.MatchesMimeType(detection.MimeType, t));
            if (isDenied)
            {
                errors.Add(ValidationError.DeniedType(detection.MimeType));
            }
        }

        // Check allowed categories
        if (_allowedCategories != MimeCategory.All && detection.Category != MimeCategory.Unknown)
        {
            if ((detection.Category & _allowedCategories) == 0)
            {
                errors.Add(ValidationError.DisallowedCategory(detection.Category, _allowedCategories));
            }
        }

        // Check denied categories
        if (_deniedCategories != MimeCategory.Unknown && detection.Category != MimeCategory.Unknown)
        {
            if ((detection.Category & _deniedCategories) != 0)
            {
                errors.Add(ValidationError.DisallowedCategory(detection.Category));
            }
        }

        // Check file size
        if (_maxSize.HasValue && _fileSize.HasValue && _fileSize.Value > _maxSize.Value)
        {
            errors.Add(ValidationError.FileTooLarge(_fileSize.Value, _maxSize.Value));
        }

        if (_minSize.HasValue && _fileSize.HasValue && _fileSize.Value < _minSize.Value)
        {
            errors.Add(ValidationError.FileTooSmall(_fileSize.Value, _minSize.Value));
        }

        // Check extension match
        if (_validateExtension && _fileName != null && detection.Extension != null)
        {
            var actualExtension = Path.GetExtension(_fileName);
            if (!string.IsNullOrEmpty(actualExtension))
            {
                var expectedExtensions = new List<string> { detection.Extension };
                expectedExtensions.AddRange(detection.AlternativeExtensions);

                bool extensionMatches = expectedExtensions.Any(e =>
                    e.Equals(actualExtension, StringComparison.OrdinalIgnoreCase));

                if (!extensionMatches)
                {
                    errors.Add(ValidationError.ExtensionMismatch(
                        actualExtension,
                        detection.Extension,
                        detection.MimeType ?? "unknown"));
                }
            }
        }

        if (errors.Count > 0)
        {
            return ValidationResult.Failure(detection, errors, _fileSize, _fileName);
        }

        return ValidationResult.Success(detection, _fileSize, _fileName);
    }

    #endregion
}

