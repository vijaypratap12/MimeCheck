namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for font file formats.
/// </summary>
internal static class FontSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // TrueType Font
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Ttf,
            Category = MimeCategory.Font,
            MagicBytes = [0x00, 0x01, 0x00, 0x00],
            Extension = ".ttf",
            Description = "TrueType Font"
        };

        // OpenType Font (with CFF data)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Otf,
            Category = MimeCategory.Font,
            MagicBytes = [0x4F, 0x54, 0x54, 0x4F], // OTTO
            Extension = ".otf",
            Description = "OpenType Font"
        };

        // WOFF (Web Open Font Format)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Woff,
            Category = MimeCategory.Font,
            MagicBytes = [0x77, 0x4F, 0x46, 0x46], // wOFF
            Extension = ".woff",
            Description = "Web Open Font Format"
        };

        // WOFF2 (Web Open Font Format 2)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Woff2,
            Category = MimeCategory.Font,
            MagicBytes = [0x77, 0x4F, 0x46, 0x32], // wOF2
            Extension = ".woff2",
            Description = "Web Open Font Format 2"
        };

        // EOT (Embedded OpenType)
        yield return new MimeSignature
        {
            MimeType = "application/vnd.ms-fontobject",
            Category = MimeCategory.Font,
            MagicBytes = [0x00, 0x00, 0x01],
            Extension = ".eot",
            Description = "Embedded OpenType Font",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x4C, 0x50], // LP
                    Offset = 34
                }
            ]
        };

        // TrueType Collection
        yield return new MimeSignature
        {
            MimeType = "font/collection",
            Category = MimeCategory.Font,
            MagicBytes = [0x74, 0x74, 0x63, 0x66], // ttcf
            Extension = ".ttc",
            Description = "TrueType Collection"
        };
    }
}

