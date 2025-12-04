namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for audio file formats.
/// </summary>
internal static class AudioSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // MP3 (ID3v2)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp3,
            Category = MimeCategory.Audio,
            MagicBytes = [0x49, 0x44, 0x33], // ID3
            Extension = ".mp3",
            Description = "MP3 Audio (ID3v2)"
        };

        // MP3 (Frame sync)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp3,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xFB],
            Extension = ".mp3",
            Description = "MP3 Audio (Frame Sync)"
        };

        // MP3 (Frame sync variant)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp3,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xFA],
            Extension = ".mp3",
            Description = "MP3 Audio (Frame Sync)"
        };

        // MP3 (Frame sync variant)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp3,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xF3],
            Extension = ".mp3",
            Description = "MP3 Audio (Frame Sync)"
        };

        // MP3 (Frame sync variant)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Mp3,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xF2],
            Extension = ".mp3",
            Description = "MP3 Audio (Frame Sync)"
        };

        // WAV
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Wav,
            Category = MimeCategory.Audio,
            MagicBytes = [0x52, 0x49, 0x46, 0x46], // RIFF
            Extension = ".wav",
            Description = "WAV Audio",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x57, 0x41, 0x56, 0x45], // WAVE
                    Offset = 8
                }
            ]
        };

        // FLAC
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Flac,
            Category = MimeCategory.Audio,
            MagicBytes = [0x66, 0x4C, 0x61, 0x43], // fLaC
            Extension = ".flac",
            Description = "FLAC Audio"
        };

        // OGG (Vorbis/Opus)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Ogg,
            Category = MimeCategory.Audio,
            MagicBytes = [0x4F, 0x67, 0x67, 0x53], // OggS
            Extension = ".ogg",
            AlternativeExtensions = [".oga", ".opus"],
            Description = "OGG Audio"
        };

        // AAC (ADTS)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Aac,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xF1],
            Extension = ".aac",
            Description = "AAC Audio (ADTS)"
        };

        // AAC (ADTS variant)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Aac,
            Category = MimeCategory.Audio,
            MagicBytes = [0xFF, 0xF9],
            Extension = ".aac",
            Description = "AAC Audio (ADTS)"
        };

        // M4A (MPEG-4 Audio)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.M4a,
            Category = MimeCategory.Audio,
            MagicBytes = [0x00, 0x00, 0x00],
            Extension = ".m4a",
            Description = "M4A Audio",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41], // ftypM4A
                    Offset = 4
                }
            ]
        };

        // MIDI
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Midi,
            Category = MimeCategory.Audio,
            MagicBytes = [0x4D, 0x54, 0x68, 0x64], // MThd
            Extension = ".mid",
            AlternativeExtensions = [".midi"],
            Description = "MIDI Audio"
        };

        // AIFF
        yield return new MimeSignature
        {
            MimeType = "audio/aiff",
            Category = MimeCategory.Audio,
            MagicBytes = [0x46, 0x4F, 0x52, 0x4D], // FORM
            Extension = ".aiff",
            AlternativeExtensions = [".aif"],
            Description = "AIFF Audio",
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x41, 0x49, 0x46, 0x46], // AIFF
                    Offset = 8
                }
            ]
        };

        // WMA (Windows Media Audio)
        yield return new MimeSignature
        {
            MimeType = "audio/x-ms-wma",
            Category = MimeCategory.Audio,
            MagicBytes = [0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11],
            Extension = ".wma",
            Description = "Windows Media Audio"
        };

        // AMR
        yield return new MimeSignature
        {
            MimeType = "audio/amr",
            Category = MimeCategory.Audio,
            MagicBytes = [0x23, 0x21, 0x41, 0x4D, 0x52], // #!AMR
            Extension = ".amr",
            Description = "AMR Audio"
        };

        // APE (Monkey's Audio)
        yield return new MimeSignature
        {
            MimeType = "audio/x-ape",
            Category = MimeCategory.Audio,
            MagicBytes = [0x4D, 0x41, 0x43, 0x20], // MAC 
            Extension = ".ape",
            Description = "Monkey's Audio"
        };
    }
}

