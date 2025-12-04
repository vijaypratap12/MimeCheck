namespace MimeCheck.Signatures.Categories;

/// <summary>
/// Contains magic byte signatures for document file formats.
/// </summary>
internal static class DocumentSignatures
{
    public static IEnumerable<MimeSignature> GetAll()
    {
        // PDF
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Pdf,
            Category = MimeCategory.Document,
            MagicBytes = [0x25, 0x50, 0x44, 0x46, 0x2D], // %PDF-
            Extension = ".pdf",
            Description = "PDF Document"
        };

        // DOCX - Check for [Content_Types].xml with word/ reference
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Docx,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04], // PK..
            Extension = ".docx",
            Description = "Microsoft Word Document (OOXML)",
            Priority = 100,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x77, 0x6F, 0x72, 0x64, 0x2F], // word/
                    SearchAnywhere = true,
                    SearchLimit = 2000
                }
            ]
        };

        // XLSX
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Xlsx,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".xlsx",
            Description = "Microsoft Excel Spreadsheet (OOXML)",
            Priority = 100,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x78, 0x6C, 0x2F], // xl/
                    SearchAnywhere = true,
                    SearchLimit = 2000
                }
            ]
        };

        // PPTX
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Pptx,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".pptx",
            Description = "Microsoft PowerPoint Presentation (OOXML)",
            Priority = 100,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x70, 0x70, 0x74, 0x2F], // ppt/
                    SearchAnywhere = true,
                    SearchLimit = 2000
                }
            ]
        };

        // OLE Compound Document (DOC, XLS, PPT, MSI)
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Doc,
            Category = MimeCategory.Document,
            MagicBytes = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1],
            Extension = ".doc",
            Description = "Microsoft Word Document (OLE)",
            Priority = 50
        };

        yield return new MimeSignature
        {
            MimeType = MimeTypes.Xls,
            Category = MimeCategory.Document,
            MagicBytes = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1],
            Extension = ".xls",
            Description = "Microsoft Excel Spreadsheet (OLE)",
            Priority = 50
        };

        yield return new MimeSignature
        {
            MimeType = MimeTypes.Ppt,
            Category = MimeCategory.Document,
            MagicBytes = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1],
            Extension = ".ppt",
            Description = "Microsoft PowerPoint Presentation (OLE)",
            Priority = 50
        };

        // RTF
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Rtf,
            Category = MimeCategory.Document,
            MagicBytes = [0x7B, 0x5C, 0x72, 0x74, 0x66], // {\rtf
            Extension = ".rtf",
            Description = "Rich Text Format"
        };

        // OpenDocument formats (ODT, ODS, ODP) - ZIP-based
        yield return new MimeSignature
        {
            MimeType = MimeTypes.Odt,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".odt",
            Description = "OpenDocument Text",
            Priority = 90,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x6D, 0x69, 0x6D, 0x65, 0x74, 0x79, 0x70, 0x65], // mimetype
                    SearchAnywhere = true,
                    SearchLimit = 100
                }
            ]
        };

        yield return new MimeSignature
        {
            MimeType = MimeTypes.Ods,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".ods",
            Description = "OpenDocument Spreadsheet",
            Priority = 90,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x6D, 0x69, 0x6D, 0x65, 0x74, 0x79, 0x70, 0x65],
                    SearchAnywhere = true,
                    SearchLimit = 100
                }
            ]
        };

        yield return new MimeSignature
        {
            MimeType = MimeTypes.Odp,
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".odp",
            Description = "OpenDocument Presentation",
            Priority = 90,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x6D, 0x69, 0x6D, 0x65, 0x74, 0x79, 0x70, 0x65],
                    SearchAnywhere = true,
                    SearchLimit = 100
                }
            ]
        };

        // EPUB (ZIP-based)
        yield return new MimeSignature
        {
            MimeType = "application/epub+zip",
            Category = MimeCategory.Document,
            MagicBytes = [0x50, 0x4B, 0x03, 0x04],
            Extension = ".epub",
            Description = "EPUB eBook",
            Priority = 95,
            AdditionalChecks =
            [
                new AdditionalCheck
                {
                    Bytes = [0x6D, 0x69, 0x6D, 0x65, 0x74, 0x79, 0x70, 0x65, 0x61, 0x70, 0x70, 0x6C, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x2F, 0x65, 0x70, 0x75, 0x62], // mimetypeapplication/epub
                    Offset = 30,
                    SearchAnywhere = true,
                    SearchLimit = 100
                }
            ]
        };
    }
}
