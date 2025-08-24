namespace DriverGuide.UI.Utils
{
    public static class FileExtensionUtils
    {
        private static readonly Dictionary<string, string> MimeTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            { ".mp4", "video/mp4" },
            { ".webm", "video/webm" },
            { ".ogg", "video/ogg" },
            { ".wmv", "video/x-ms-wmv" },
            { ".avi", "video/x-msvideo" },
            { ".mov", "video/quicktime" },
            { ".mkv", "video/x-matroska" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" },
            { ".mp3", "audio/mpeg" },
            { ".wav", "audio/wav" },
            { ".aac", "audio/aac" },
            { ".flac", "audio/flac" }
        };

        public static string GetMimeType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "application/octet-stream";

            var ext = Path.GetExtension(fileName);
            if (ext != null && MimeTypes.TryGetValue(ext, out var mime))
                return mime;

            return "application/octet-stream";
        }
    }
}
