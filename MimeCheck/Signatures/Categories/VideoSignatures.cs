namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for video file formats.
/// </summary>
internal static class VideoSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // MP4 (various ftyp brands)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp4,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".mp4",
            AlternativeExtensions = [".m4v"],
            Description = "MP4 Video",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D], // ftypisom
                    Offset = 4
                }
            ]
        };

        // MP4 (mp42)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp4,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".mp4",
            Description = "MP4 Video (mp42)",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32], // ftypmp42
                    Offset = 4
                }
            ]
        };

        // MP4 (mp41)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp4,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".mp4",
            Description = "MP4 Video (mp41)",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x31], // ftypmp41
                    Offset = 4
                }
            ]
        };

        // AVI
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Avi,
            Category = MimeCategory.Video,
            MagicBytes = [0x52, 0x49, 0x46, 0x46], // RIFF
            Extension = ".avi",
            Description = "AVI Video",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x41, 0x56, 0x49, 0x20], // AVI 
                    Offset = 8
                }
            ]
        };

        // MKV (Matroska)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mkv,
            Category = MimeCategory.Video,
            MagicBytes = [0x1A, 0x45, 0xDF, 0xA3],
            Extension = ".mkv",
            AlternativeExtensions = [".mka", ".mks"],
            Description = "Matroska Video"
        };

        // MOV (QuickTime)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mov,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".mov",
            Description = "QuickTime Video",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20], // ftypqt  
                    Offset = 4
                }
            ]
        };

        // MOV (moov atom at start)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mov,
            Category = MimeCategory.Video,
            MagicBytes = [0x6D, 0x6F, 0x6F, 0x76], // moov
            Offset = 4,
            Extension = ".mov",
            Description = "QuickTime Video (moov)"
        };

        // WebM
        yield return new MimeSignature
        {
            MimeType = MimeTypes.WebmVideo,
            Category = MimeCategory.Video,
            MagicBytes = [0x1A, 0x45, 0xDF, 0xA3],
            Extension = ".webm",
            Description = "WebM Video",
            Priority = 50,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x77, 0x65, 0x62, 0x6D], // webm
                    SearchAnywhere = true,
                    SearchLimit = 64
                }
            ]
        };

        // FLV
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Flv,
            Category = MimeCategory.Video,
            MagicBytes = [0x46, 0x4C, 0x56, 0x01], // FLV.
            Extension = ".flv",
            Description = "Flash Video"
        };

        // WMV (Windows Media Video)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Wmv,
            Category = MimeCategory.Video,
            MagicBytes = [0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11],
            Extension = ".wmv",
            AlternativeExtensions = [".asf"],
            Description = "Windows Media Video"
        };

        // MPEG
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mpeg,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x01, 0xBA],
            Extension = ".mpg",
            AlternativeExtensions = [".mpeg", ".mpe"],
            Description = "MPEG Video"
        };

        // MPEG (variant)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mpeg,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x01, 0xB3],
            Extension = ".mpg",
            AlternativeExtensions = [".mpeg"],
            Description = "MPEG Video"
        };

        // 3GP
        yield return new MimeSignature
        {
            MimeType = MimeTypes.ThreeGp,
            Category = MimeCategory.Video,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".3gp",
            AlternativeExtensions = [".3g2"],
            Description = "3GP Video",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70], // ftyp3gp
                    Offset = 4
                }
            ]
        };

        // OGV (OGG Video)
        yield return new MimeSignature
        {
            MimeType = "video/ogg",
            Category = MimeCategory.Video,
            MagicBytes = [0x4F, 0x67, 0x67, 0x53], // OggS
            Extension = ".ogv",
            Description = "OGG Video"
        };

        // M2TS (Blu-ray MPEG-2 Transport Stream)
        yield return new MimeSignature
        {
            MimeType = "video/mp2t",
            Category = MimeCategory.Video,
            MagicBytes = [0x47],
            Extension = ".m2ts",
            AlternativeExtensions = [".mts", ".ts"],
            Description = "MPEG-2 Transport Stream"
        };
    }
}
