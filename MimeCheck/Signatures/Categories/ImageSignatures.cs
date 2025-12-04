namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for image file formats.
/// </summary>
internal static class ImageSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // JPEG
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Jpeg,
            Category = MimeCategory.Image,
            MagicBytes = [0xFF, 0xD8, 0xFF],
            Extension = ".jpg",
            AlternativeExtensions = [".jpeg", ".jpe", ".jfif"],
            Description = "JPEG Image"
        };

        // PNG
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Png,
            Category = MimeCategory.Image,
            MagicBytes = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Extension = ".png",
            Description = "PNG Image"
        };

        // GIF87a
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Gif,
            Category = MimeCategory.Image,
            MagicBytes = [0x47, 0x49, 0x46, 0x38, 0x37, 0x61], // GIF87a
            Extension = ".gif",
            Description = "GIF Image (87a)"
        };

        // GIF89a
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Gif,
            Category = MimeCategory.Image,
            MagicBytes = [0x47, 0x49, 0x46, 0x38, 0x39, 0x61], // GIF89a
            Extension = ".gif",
            Description = "GIF Image (89a)"
        };

        // BMP
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Bmp,
            Category = MimeCategory.Image,
            MagicBytes = [0x42, 0x4D], // BM
            Extension = ".bmp",
            AlternativeExtensions = [".dib"],
            Description = "BMP Image"
        };

        // WebP
        yield return new MimeSignature
        {
            MimeType = MimeTypes.WebP,
            Category = MimeCategory.Image,
            MagicBytes = [0x52, 0x49, 0x46, 0x46], // RIFF
            Extension = ".webp",
            Description = "WebP Image",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x57, 0x45, 0x42, 0x50], // WEBP
                    Offset = 8
                }
            ]
        };

        // TIFF (Little Endian)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Tiff,
            Category = MimeCategory.Image,
            MagicBytes = [0x49, 0x49, 0x2A, 0x00], // II*.
            Extension = ".tiff",
            AlternativeExtensions = [".tif"],
            Description = "TIFF Image (Little Endian)"
        };

        // TIFF (Big Endian)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Tiff,
            Category = MimeCategory.Image,
            MagicBytes = [0x4D, 0x4D, 0x00, 0x2A], // MM.*
            Extension = ".tiff",
            AlternativeExtensions = [".tif"],
            Description = "TIFF Image (Big Endian)"
        };

        // ICO
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Ico,
            Category = MimeCategory.Image,
            MagicBytes = [0x00, 0x00, 0x01, 0x00],
            Extension = ".ico",
            Description = "ICO Icon"
        };

        // CUR (Cursor)
        yield return new MimeSignature
        {
            MimeType = "image/x-cursor",
            Category = MimeCategory.Image,
            MagicBytes = [0x00, 0x00, 0x02, 0x00],
            Extension = ".cur",
            Description = "Windows Cursor"
        };

        // HEIC/HEIF (ftyp heic)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Heic,
            Category = MimeCategory.Image,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".heic",
            AlternativeExtensions = [".heif"],
            Description = "HEIC Image",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x68, 0x65, 0x69, 0x63], // ftypheic
                    Offset = 4
                }
            ]
        };

        // AVIF
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Avif,
            Category = MimeCategory.Image,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".avif",
            Description = "AVIF Image",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66], // ftypavif
                    Offset = 4
                }
            ]
        };

        // PSD (Photoshop)
        yield return new MimeSignature
        {
            MimeType = "image/vnd.adobe.photoshop",
            Category = MimeCategory.Image,
            MagicBytes = [0x38, 0x42, 0x50, 0x53], // 8BPS
            Extension = ".psd",
            Description = "Adobe Photoshop Document"
        };

        // JPEG 2000
        yield return new MimeSignature
        {
            MimeType = "image/jp2",
            Category = MimeCategory.Image,
            MagicBytes = [0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20],
            Extension = ".jp2",
            AlternativeExtensions = [".j2k", ".jpx"],
            Description = "JPEG 2000 Image"
        };
    }
}
