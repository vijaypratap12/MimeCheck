namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for other file formats (database, text, etc.).
/// </summary>
internal static class OtherSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // SQLite Database
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Sqlite,
            Category = MimeCategory.Database,
            MagicBytes = [0x53, 0x51, 0x4C, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x20, 0x33, 0x00], // SQLite format 3.
            Extension = ".sqlite",
            AlternativeExtensions = [".db", ".sqlite3"],
            Description = "SQLite Database"
        };

        // XML (with declaration)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Xml,
            Category = MimeCategory.Text,
            MagicBytes = [0x3C, 0x3F, 0x78, 0x6D, 0x6C], // <?xml
            Extension = ".xml",
            Description = "XML Document"
        };

        // HTML (doctype)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Html,
            Category = MimeCategory.Text,
            MagicBytes = [0x3C, 0x21, 0x44, 0x4F, 0x43, 0x54, 0x59, 0x50, 0x45], // <!DOCTYPE
            Extension = ".html",
            AlternativeExtensions = [".htm"],
            Description = "HTML Document"
        };

        // HTML (html tag)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Html,
            Category = MimeCategory.Text,
            MagicBytes = [0x3C, 0x68, 0x74, 0x6D, 0x6C], // <html
            Extension = ".html",
            AlternativeExtensions = [".htm"],
            Description = "HTML Document"
        };

        // SVG
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Svg,
            Category = MimeCategory.Image,
            MagicBytes = [0x3C, 0x73, 0x76, 0x67], // <svg
            Extension = ".svg",
            Description = "SVG Image"
        };

        // SVG (with XML declaration)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Svg,
            Category = MimeCategory.Image,
            MagicBytes = [0x3C, 0x3F, 0x78, 0x6D, 0x6C], // <?xml
            Extension = ".svg",
            Description = "SVG Image (XML)",
            Priority = -10, // Lower priority, needs additional check
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x3C, 0x73, 0x76, 0x67], // <svg
                    SearchAnywhere = true,
                    SearchLimit = 1024
                }
            ]
        };

        // JSON (starts with {)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Json,
            Category = MimeCategory.Text,
            MagicBytes = [0x7B], // {
            Extension = ".json",
            Description = "JSON Document",
            Priority = -100 // Very low priority, too generic
        };

        // JSON (starts with [)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Json,
            Category = MimeCategory.Text,
            MagicBytes = [0x5B], // [
            Extension = ".json",
            Description = "JSON Array",
            Priority = -100
        };

        // UTF-8 BOM
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Text,
            Category = MimeCategory.Text,
            MagicBytes = [0xEF, 0xBB, 0xBF],
            Extension = ".txt",
            Description = "UTF-8 Text (BOM)",
            Priority = -50
        };

        // UTF-16 LE BOM
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Text,
            Category = MimeCategory.Text,
            MagicBytes = [0xFF, 0xFE],
            Extension = ".txt",
            Description = "UTF-16 LE Text (BOM)",
            Priority = -50
        };

        // UTF-16 BE BOM
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Text,
            Category = MimeCategory.Text,
            MagicBytes = [0xFE, 0xFF],
            Extension = ".txt",
            Description = "UTF-16 BE Text (BOM)",
            Priority = -50
        };

        // PostScript
        yield return new MimeSignature
        {
            MimeType = "application/postscript",
            Category = MimeCategory.Document,
            MagicBytes = [0x25, 0x21, 0x50, 0x53], // %!PS
            Extension = ".ps",
            AlternativeExtensions = [".eps"],
            Description = "PostScript"
        };

        // iCalendar
        yield return new MimeSignature
        {
            MimeType = "text/calendar",
            Category = MimeCategory.Text,
            MagicBytes = [0x42, 0x45, 0x47, 0x49, 0x4E, 0x3A, 0x56, 0x43, 0x41, 0x4C, 0x45, 0x4E, 0x44, 0x41, 0x52], // BEGIN:VCALENDAR
            Extension = ".ics",
            Description = "iCalendar"
        };

        // vCard
        yield return new MimeSignature
        {
            MimeType = "text/vcard",
            Category = MimeCategory.Text,
            MagicBytes = [0x42, 0x45, 0x47, 0x49, 0x4E, 0x3A, 0x56, 0x43, 0x41, 0x52, 0x44], // BEGIN:VCARD
            Extension = ".vcf",
            Description = "vCard"
        };
    }
}
