namespace MimeCheck;

/// <summary>
/// Contains constants for common MIME types.
/// </summary>
public static class MimeTypes
{
    #region Images

    /// <summary>JPEG image (image/jpeg)</summary>
    public const string Jpeg = "image/jpeg";

    /// <summary>PNG image (image/png)</summary>
    public const string Png = "image/png";

    /// <summary>GIF image (image/gif)</summary>
    public const string Gif = "image/gif";

    /// <summary>BMP image (image/bmp)</summary>
    public const string Bmp = "image/bmp";

    /// <summary>WebP image (image/webp)</summary>
    public const string WebP = "image/webp";

    /// <summary>TIFF image (image/tiff)</summary>
    public const string Tiff = "image/tiff";

    /// <summary>ICO image (image/x-icon)</summary>
    public const string Ico = "image/x-icon";

    /// <summary>SVG image (image/svg+xml)</summary>
    public const string Svg = "image/svg+xml";

    /// <summary>HEIC image (image/heic)</summary>
    public const string Heic = "image/heic";

    /// <summary>AVIF image (image/avif)</summary>
    public const string Avif = "image/avif";

    #endregion

    #region Documents

    /// <summary>PDF document (application/pdf)</summary>
    public const string Pdf = "application/pdf";

    /// <summary>Microsoft Word DOCX (application/vnd.openxmlformats-officedocument.wordprocessingml.document)</summary>
    public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    /// <summary>Microsoft Excel XLSX (application/vnd.openxmlformats-officedocument.spreadsheetml.sheet)</summary>
    public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>Microsoft PowerPoint PPTX (application/vnd.openxmlformats-officedocument.presentationml.presentation)</summary>
    public const string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";

    /// <summary>Microsoft Word DOC (application/msword)</summary>
    public const string Doc = "application/msword";

    /// <summary>Microsoft Excel XLS (application/vnd.ms-excel)</summary>
    public const string Xls = "application/vnd.ms-excel";

    /// <summary>Microsoft PowerPoint PPT (application/vnd.ms-powerpoint)</summary>
    public const string Ppt = "application/vnd.ms-powerpoint";

    /// <summary>Rich Text Format (application/rtf)</summary>
    public const string Rtf = "application/rtf";

    /// <summary>OpenDocument Text (application/vnd.oasis.opendocument.text)</summary>
    public const string Odt = "application/vnd.oasis.opendocument.text";

    /// <summary>OpenDocument Spreadsheet (application/vnd.oasis.opendocument.spreadsheet)</summary>
    public const string Ods = "application/vnd.oasis.opendocument.spreadsheet";

    /// <summary>OpenDocument Presentation (application/vnd.oasis.opendocument.presentation)</summary>
    public const string Odp = "application/vnd.oasis.opendocument.presentation";

    #endregion

    #region Archives

    /// <summary>ZIP archive (application/zip)</summary>
    public const string Zip = "application/zip";

    /// <summary>RAR archive (application/vnd.rar)</summary>
    public const string Rar = "application/vnd.rar";

    /// <summary>7-Zip archive (application/x-7z-compressed)</summary>
    public const string SevenZip = "application/x-7z-compressed";

    /// <summary>TAR archive (application/x-tar)</summary>
    public const string Tar = "application/x-tar";

    /// <summary>GZIP archive (application/gzip)</summary>
    public const string Gzip = "application/gzip";

    /// <summary>BZIP2 archive (application/x-bzip2)</summary>
    public const string Bzip2 = "application/x-bzip2";

    /// <summary>XZ archive (application/x-xz)</summary>
    public const string Xz = "application/x-xz";

    #endregion

    #region Audio

    /// <summary>MP3 audio (audio/mpeg)</summary>
    public const string Mp3 = "audio/mpeg";

    /// <summary>WAV audio (audio/wav)</summary>
    public const string Wav = "audio/wav";

    /// <summary>FLAC audio (audio/flac)</summary>
    public const string Flac = "audio/flac";

    /// <summary>OGG audio (audio/ogg)</summary>
    public const string Ogg = "audio/ogg";

    /// <summary>AAC audio (audio/aac)</summary>
    public const string Aac = "audio/aac";

    /// <summary>M4A audio (audio/mp4)</summary>
    public const string M4a = "audio/mp4";

    /// <summary>MIDI audio (audio/midi)</summary>
    public const string Midi = "audio/midi";

    /// <summary>WebM audio (audio/webm)</summary>
    public const string WebmAudio = "audio/webm";

    #endregion

    #region Video

    /// <summary>MP4 video (video/mp4)</summary>
    public const string Mp4 = "video/mp4";

    /// <summary>AVI video (video/x-msvideo)</summary>
    public const string Avi = "video/x-msvideo";

    /// <summary>MKV video (video/x-matroska)</summary>
    public const string Mkv = "video/x-matroska";

    /// <summary>MOV video (video/quicktime)</summary>
    public const string Mov = "video/quicktime";

    /// <summary>WebM video (video/webm)</summary>
    public const string WebmVideo = "video/webm";

    /// <summary>FLV video (video/x-flv)</summary>
    public const string Flv = "video/x-flv";

    /// <summary>WMV video (video/x-ms-wmv)</summary>
    public const string Wmv = "video/x-ms-wmv";

    /// <summary>MPEG video (video/mpeg)</summary>
    public const string Mpeg = "video/mpeg";

    /// <summary>3GP video (video/3gpp)</summary>
    public const string ThreeGp = "video/3gpp";

    #endregion

    #region Executables

    /// <summary>Windows Executable (application/x-msdownload)</summary>
    public const string Exe = "application/x-msdownload";

    /// <summary>Windows DLL (application/x-msdownload)</summary>
    public const string Dll = "application/x-msdownload";

    /// <summary>Windows MSI Installer (application/x-msi)</summary>
    public const string Msi = "application/x-msi";

    /// <summary>Android APK (application/vnd.android.package-archive)</summary>
    public const string Apk = "application/vnd.android.package-archive";

    /// <summary>macOS DMG (application/x-apple-diskimage)</summary>
    public const string Dmg = "application/x-apple-diskimage";

    /// <summary>Java JAR (application/java-archive)</summary>
    public const string Jar = "application/java-archive";

    /// <summary>ELF executable (application/x-executable)</summary>
    public const string Elf = "application/x-executable";

    /// <summary>Mach-O executable (application/x-mach-binary)</summary>
    public const string MachO = "application/x-mach-binary";

    #endregion

    #region Text/Code

    /// <summary>Plain text (text/plain)</summary>
    public const string Text = "text/plain";

    /// <summary>HTML (text/html)</summary>
    public const string Html = "text/html";

    /// <summary>CSS (text/css)</summary>
    public const string Css = "text/css";

    /// <summary>JavaScript (application/javascript)</summary>
    public const string JavaScript = "application/javascript";

    /// <summary>JSON (application/json)</summary>
    public const string Json = "application/json";

    /// <summary>XML (application/xml)</summary>
    public const string Xml = "application/xml";

    #endregion

    #region Fonts

    /// <summary>TrueType Font (font/ttf)</summary>
    public const string Ttf = "font/ttf";

    /// <summary>OpenType Font (font/otf)</summary>
    public const string Otf = "font/otf";

    /// <summary>WOFF Font (font/woff)</summary>
    public const string Woff = "font/woff";

    /// <summary>WOFF2 Font (font/woff2)</summary>
    public const string Woff2 = "font/woff2";

    #endregion

    #region Database

    /// <summary>SQLite database (application/x-sqlite3)</summary>
    public const string Sqlite = "application/x-sqlite3";

    #endregion

    #region Other

    /// <summary>Binary/Octet stream (application/octet-stream)</summary>
    public const string OctetStream = "application/octet-stream";

    /// <summary>WebAssembly (application/wasm)</summary>
    public const string Wasm = "application/wasm";

    #endregion
}

