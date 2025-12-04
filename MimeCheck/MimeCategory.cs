namespace MimeCheck;

/// <summary>
/// Represents the category of a file based on its MIME type.
/// </summary>
[Flags]
public enum MimeCategory
{
    /// <summary>
    /// Unknown or unrecognized file type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Image files (JPEG, PNG, GIF, BMP, WebP, etc.).
    /// </summary>
    Image = 1 << 0,

    /// <summary>
    /// Document files (PDF, DOCX, XLSX, PPTX, etc.).
    /// </summary>
    Document = 1 << 1,

    /// <summary>
    /// Archive files (ZIP, RAR, 7Z, TAR, GZ, etc.).
    /// </summary>
    Archive = 1 << 2,

    /// <summary>
    /// Audio files (MP3, WAV, FLAC, OGG, etc.).
    /// </summary>
    Audio = 1 << 3,

    /// <summary>
    /// Video files (MP4, AVI, MKV, MOV, etc.).
    /// </summary>
    Video = 1 << 4,

    /// <summary>
    /// Executable files (EXE, DLL, MSI, etc.).
    /// </summary>
    Executable = 1 << 5,

    /// <summary>
    /// Font files (TTF, OTF, WOFF, etc.).
    /// </summary>
    Font = 1 << 6,

    /// <summary>
    /// Text-based files (TXT, JSON, XML, HTML, etc.).
    /// </summary>
    Text = 1 << 7,

    /// <summary>
    /// Database files (SQLite, etc.).
    /// </summary>
    Database = 1 << 8,

    /// <summary>
    /// All media types (Image, Audio, Video).
    /// </summary>
    Media = Image | Audio | Video,

    /// <summary>
    /// All file types.
    /// </summary>
    All = Image | Document | Archive | Audio | Video | Executable | Font | Text | Database
}

