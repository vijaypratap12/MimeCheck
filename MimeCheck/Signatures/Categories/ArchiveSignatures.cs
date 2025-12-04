namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for archive file formats.
/// </summary>
internal static class ArchiveSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // ZIP (generic - lower priority than specific ZIP-based formats)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Zip,
            Category = MimeCategory.Archive,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04], // PK..
            Extension = ".zip",
            Description = "ZIP Archive",
            Priority = 10
        };

        // ZIP (empty archive)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Zip,
            Category = MimeCategory.Archive,
            MagicBytes = [0x50, 0x4B, 0x05, 0x06],
            Extension = ".zip",
            Description = "ZIP Archive (Empty)",
            Priority = 10
        };

        // ZIP (spanned archive)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Zip,
            Category = MimeCategory.Archive,
            MagicBytes = [0x50, 0x4B, 0x07, 0x08],
            Extension = ".zip",
            Description = "ZIP Archive (Spanned)",
            Priority = 10
        };

        // RAR (v1.5+)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Rar,
            Category = MimeCategory.Archive,
            MagicBytes = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00], // Rar!...
            Extension = ".rar",
            Description = "RAR Archive (v1.5+)"
        };

        // RAR (v5.0+)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Rar,
            Category = MimeCategory.Archive,
            MagicBytes = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00], // Rar!....
            Extension = ".rar",
            Description = "RAR Archive (v5.0+)"
        };

        // 7-Zip
        yield return new MimeSignature
        {
            MimeType = MimeTypes.SevenZip,
            Category = MimeCategory.Archive,
            MagicBytes = [0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C], // 7z...
            Extension = ".7z",
            Description = "7-Zip Archive"
        };

        // TAR (POSIX)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Tar,
            Category = MimeCategory.Archive,
            MagicBytes = [0x75, 0x73, 0x74, 0x61, 0x72, 0x00, 0x30, 0x30], // ustar.00
            Offset = 257,
            Extension = ".tar",
            Description = "TAR Archive (POSIX)"
        };

        // TAR (GNU)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Tar,
            Category = MimeCategory.Archive,
            MagicBytes = [0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20, 0x00], // ustar  .
            Offset = 257,
            Extension = ".tar",
            Description = "TAR Archive (GNU)"
        };

        // GZIP
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Gzip,
            Category = MimeCategory.Archive,
            MagicBytes = [0x1F, 0x8B],
            Extension = ".gz",
            AlternativeExtensions = [".gzip", ".tgz"],
            Description = "GZIP Archive"
        };

        // BZIP2
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Bzip2,
            Category = MimeCategory.Archive,
            MagicBytes = [0x42, 0x5A, 0x68], // BZh
            Extension = ".bz2",
            AlternativeExtensions = [".bzip2", ".tbz2"],
            Description = "BZIP2 Archive"
        };

        // XZ
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Xz,
            Category = MimeCategory.Archive,
            MagicBytes = [0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00], // .7zXZ.
            Extension = ".xz",
            AlternativeExtensions = [".txz"],
            Description = "XZ Archive"
        };

        // LZMA
        yield return new MimeSignature
        {
            MimeType = "application/x-lzma",
            Category = MimeCategory.Archive,
            MagicBytes = [0x5D, 0x00, 0x00],
            Extension = ".lzma",
            Description = "LZMA Archive"
        };

        // Zstandard
        yield return new MimeSignature
        {
            MimeType = "application/zstd",
            Category = MimeCategory.Archive,
            MagicBytes = [0x28, 0xB5, 0x2F, 0xFD],
            Extension = ".zst",
            AlternativeExtensions = [".zstd"],
            Description = "Zstandard Archive"
        };

        // LZ4
        yield return new MimeSignature
        {
            MimeType = "application/x-lz4",
            Category = MimeCategory.Archive,
            MagicBytes = [0x04, 0x22, 0x4D, 0x18],
            Extension = ".lz4",
            Description = "LZ4 Archive"
        };

        // CAB (Microsoft Cabinet)
        yield return new MimeSignature
        {
            MimeType = "application/vnd.ms-cab-compressed",
            Category = MimeCategory.Archive,
            MagicBytes = [0x4D, 0x53, 0x43, 0x46], // MSCF
            Extension = ".cab",
            Description = "Microsoft Cabinet Archive"
        };

        // ISO 9660 CD/DVD Image
        yield return new MimeSignature
        {
            MimeType = "application/x-iso9660-image",
            Category = MimeCategory.Archive,
            MagicBytes = [0x43, 0x44, 0x30, 0x30, 0x31], // CD001
            Offset = 0x8001,
            Extension = ".iso",
            Description = "ISO 9660 Disk Image"
        };
    }
}

