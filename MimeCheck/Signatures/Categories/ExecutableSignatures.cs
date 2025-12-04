namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for executable file formats.
/// </summary>
internal static class ExecutableSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // Windows PE Executable (EXE, DLL, SYS, etc.)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Exe,
            Category = MimeCategory.Executable,
            MagicBytes = [0x4D, 0x5A], // MZ
            Extension = ".exe",
            AlternativeExtensions = [".dll", ".sys", ".scr", ".com"],
            Description = "Windows Executable (PE)"
        };

        // MSI (Microsoft Installer) - OLE Compound Document
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Msi,
            Category = MimeCategory.Executable,
            MagicBytes = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1],
            Extension = ".msi",
            Description = "Microsoft Installer",
            Priority = 60
        };

        // APK (Android Package) - ZIP-based
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Apk,
            Category = MimeCategory.Executable,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".apk",
            Description = "Android Package",
            Priority = 80,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x4D, 0x61, 0x6E, 0x69, 0x66, 0x65, 0x73, 0x74, 0x2E, 0x78, 0x6D, 0x6C], // AndroidManifest.xml
                    SearchAnywhere = true,
                    SearchLimit = 4096
                }
            ]
        };

        // JAR (Java Archive) - ZIP-based
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Jar,
            Category = MimeCategory.Executable,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".jar",
            Description = "Java Archive",
            Priority = 70,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x4D, 0x45, 0x54, 0x41, 0x2D, 0x49, 0x4E, 0x46, 0x2F], // META-INF/
                    SearchAnywhere = true,
                    SearchLimit = 2000
                }
            ]
        };

        // DMG (macOS Disk Image)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Dmg,
            Category = MimeCategory.Executable,
            MagicBytes = [0x78, 0x01, 0x73, 0x0D, 0x62, 0x62, 0x60],
            Extension = ".dmg",
            Description = "macOS Disk Image"
        };

        // ELF (Linux/Unix Executable)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Elf,
            Category = MimeCategory.Executable,
            MagicBytes = [0x7F, 0x45, 0x4C, 0x46], // .ELF
            Extension = ".elf",
            AlternativeExtensions = [".so", ".o"],
            Description = "ELF Executable"
        };

        // Mach-O (macOS Executable) - 32-bit
        yield return new MimeSignature
        {
            MimeType = MimeTypes.MachO,
            Category = MimeCategory.Executable,
            MagicBytes = [0xFE, 0xED, 0xFA, 0xCE],
            Extension = ".macho",
            Description = "Mach-O Executable (32-bit)"
        };

        // Mach-O (macOS Executable) - 64-bit
        yield return new MimeSignature
        {
            MimeType = MimeTypes.MachO,
            Category = MimeCategory.Executable,
            MagicBytes = [0xFE, 0xED, 0xFA, 0xCF],
            Extension = ".macho",
            Description = "Mach-O Executable (64-bit)"
        };

        // Mach-O (macOS Executable) - 32-bit reverse
        yield return new MimeSignature
        {
            MimeType = MimeTypes.MachO,
            Category = MimeCategory.Executable,
            MagicBytes = [0xCE, 0xFA, 0xED, 0xFE],
            Extension = ".macho",
            Description = "Mach-O Executable (32-bit, reverse)"
        };

        // Mach-O (macOS Executable) - 64-bit reverse
        yield return new MimeSignature
        {
            MimeType = MimeTypes.MachO,
            Category = MimeCategory.Executable,
            MagicBytes = [0xCF, 0xFA, 0xED, 0xFE],
            Extension = ".macho",
            Description = "Mach-O Executable (64-bit, reverse)"
        };

        // Mach-O Universal Binary (Fat Binary)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.MachO,
            Category = MimeCategory.Executable,
            MagicBytes = [0xCA, 0xFE, 0xBA, 0xBE],
            Extension = ".macho",
            Description = "Mach-O Universal Binary"
        };

        // DEX (Dalvik Executable)
        yield return new MimeSignature
        {
            MimeType = "application/x-dex",
            Category = MimeCategory.Executable,
            MagicBytes = [0x64, 0x65, 0x78, 0x0A], // dex.
            Extension = ".dex",
            Description = "Dalvik Executable"
        };

        // WebAssembly
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Wasm,
            Category = MimeCategory.Executable,
            MagicBytes = [0x00, 0x61, 0x73, 0x6D], // .asm
            Extension = ".wasm",
            Description = "WebAssembly"
        };

        // Windows Shortcut (LNK)
        yield return new MimeSignature
        {
            MimeType = "application/x-ms-shortcut",
            Category = MimeCategory.Executable,
            MagicBytes = [0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00],
            Extension = ".lnk",
            Description = "Windows Shortcut"
        };

        // Batch file (DOS/Windows)
        yield return new MimeSignature
        {
            MimeType = "application/x-msdos-program",
            Category = MimeCategory.Executable,
            MagicBytes = [0x40, 0x65, 0x63, 0x68, 0x6F], // @echo
            Extension = ".bat",
            AlternativeExtensions = [".cmd"],
            Description = "Windows Batch File"
        };

        // PowerShell script (UTF-8 BOM)
        yield return new MimeSignature
        {
            MimeType = "application/x-powershell",
            Category = MimeCategory.Executable,
            MagicBytes = [0xEF, 0xBB, 0xBF], // UTF-8 BOM
            Extension = ".ps1",
            Description = "PowerShell Script"
        };
    }
}
