# Changelog

All notable changes to MimeCheck will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-05-26

A major-version bump for a purely additive feature: MIME-type **string** validation.
No existing API is renamed, removed, deprecated, or behavior-changed — your 1.x code
compiles and runs identically on 2.0.0.

### Why 2.0.0

The library previously exposed only one list of MIME types — the ones it could detect
from file bytes via the signature database. Consumers were using that list to validate
arbitrary user-supplied MIME-type strings (form fields, HTTP headers, config values),
which caused false rejections for valid but signature-less types (`text/csv`,
`multipart/form-data`) and for valid aliases that share bytes with their canonical
form (`text/xml`, `image/jpg`, `application/x-zip-compressed`). 2.0.0 separates
**"detectable from bytes"** from **"recognized MIME string"** with new APIs, while
leaving the detection path untouched.

### Added

- **`MimeValidator.IsKnownMimeType(string?)`** — returns `true` if the given string
  is a recognized MIME type. Recognizes canonical types, their aliases, and
  signature-less types. Case- and whitespace-insensitive. Use this to validate
  user-supplied MIME strings.
- **`MimeValidator.GetKnownMimeTypes()`** — enumerates every recognized MIME-type
  string (canonical + aliases + signature-less), de-duplicated.
- **`MimeSignature.Aliases`** (optional `string[]`, defaults to empty) — alternate
  MIME-type names that share a signature's byte pattern. Consulted only for
  `IsKnownMimeType` lookups; never emitted by the detector.
- **`MimeSignature.AllMimeTypes`** — computed property returning canonical type
  plus aliases.
- **`SignatureDatabase.GetAllMimeTypesIncludingAliases()`** — canonical types
  plus all registered aliases, de-duplicated.
- **`KnownMimeTypes`** (internal) — registry of signature-less MIME types that have
  no byte pattern (e.g. `text/csv`, `text/markdown`, `application/x-www-form-urlencoded`,
  `multipart/form-data`, `application/ld+json`).
- **New `MimeTypes` constants**: `TextXml`, `Csv`, `Markdown`, `Yaml`, `XHtml`,
  `JsonLd`, `FormUrlEncoded`, `MultipartFormData`.

### Aliases now recognized by `IsKnownMimeType`

Co-located with their canonical signatures in `Signatures/Categories/`:

| Canonical | Aliases |
|---|---|
| `image/jpeg` | `image/jpg`, `image/pjpeg` |
| `image/tiff` | `image/x-tiff` |
| `image/x-icon` | `image/vnd.microsoft.icon` |
| `audio/mpeg` | `audio/mp3`, `audio/x-mpeg` |
| `audio/wav` | `audio/x-wav`, `audio/wave` |
| `audio/midi` | `audio/x-midi` |
| `video/x-msvideo` | `video/avi`, `video/msvideo` |
| `video/quicktime` | `video/mov` |
| `video/x-ms-wmv` | `video/wmv` |
| `application/zip` | `application/x-zip-compressed`, `application/x-zip` |
| `application/gzip` | `application/x-gzip` |
| `application/x-tar` | `application/tar` |
| `application/msword` | `application/vnd.ms-word` |
| `application/vnd.ms-excel` | `application/excel`, `application/x-excel` |
| `application/x-msdownload` | `application/x-msdos-program`, `application/exe` |
| `application/xml` | `text/xml` |
| `application/pdf` | `application/x-pdf` |
| `font/ttf` | `application/x-font-ttf`, `application/font-sfnt` |
| `font/otf` | `application/x-font-otf` |
| `font/woff` | `application/font-woff` |
| `font/woff2` | `application/font-woff2` |

### Unchanged (backwards-compatibility guarantees)

- `MimeDetector.Detect(...)` returns the same canonical MIME type strings as in 1.x.
  Aliases are **never** returned from the detector.
- `MimeValidator.GetSupportedMimeTypes()` returns the same set as in 1.x —
  bytes-detectable canonical types only.
- All existing `MimeTypes.*` constants keep their existing string values.
- AspNetCore middleware, attributes (`AllowedMimeTypes`, `DenyMimeTypes`,
  `AllowedCategories`, `MaxFileSize`, `ValidateMimeType`), services
  (`IMimeValidationService`), and DI extensions are byte-for-byte compatible.
- `MimeValidatorBuilder` fluent API is unchanged.
- `FileExtensions`, `MimeCategory` are unchanged.

### Migration from 1.x

No code changes are required to upgrade. If you were using
`GetSupportedMimeTypes().Contains(value)` to validate user-supplied MIME strings
(the original motivation for this release), swap it for `IsKnownMimeType(value)`:

```csharp
// Before — rejects text/xml, image/jpg, text/csv, etc.
if (!MimeValidator.GetSupportedMimeTypes().Contains(value)) Reject();

// After — accepts aliases and signature-less types
if (!MimeValidator.IsKnownMimeType(value)) Reject();
```

## [1.1.0] - earlier release

See git history.
