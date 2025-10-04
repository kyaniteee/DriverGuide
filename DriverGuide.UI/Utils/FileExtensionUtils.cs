namespace DriverGuide.UI.Utils
{
    public static class FileExtensionUtils
    {
        private static readonly IReadOnlyDictionary<string, string> Map =
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [".mp4"] = "video/mp4",
                        [".m4v"] = "video/mp4",
                        [".webm"] = "video/webm",
                        [".ogg"] = "video/ogg",
                        [".ogv"] = "video/ogg",
                        [".mov"] = "video/quicktime",
                        [".mkv"] = "video/x-matroska",
                        [".avi"] = "video/x-msvideo",
                        [".wmv"] = "video/x-ms-wmv",
                        [".jpg"] = "image/jpeg",
                        [".jpeg"] = "image/jpeg",
                        [".png"] = "image/png",
                        [".gif"] = "image/gif",
                        [".bmp"] = "image/bmp",
                        [".svg"] = "image/svg+xml",
                        [".mp3"] = "audio/mpeg",
                        [".wav"] = "audio/wav",
                        [".aac"] = "audio/aac",
                        [".flac"] = "audio/flac"
                    };

        public static string GetMimeType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return "application/octet-stream";
            var ext = Path.GetExtension(fileName);
            return ext != null && Map.TryGetValue(ext, out var mime) ? mime : "application/octet-stream";
        }
    }
}
