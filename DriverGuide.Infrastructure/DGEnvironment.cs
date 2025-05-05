using Microsoft.AspNetCore.StaticFiles;

namespace DriverGuide.Infrastructure
{
    public static class DGEnvironment
    {
        public static string GetFileMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string? contentType))
                contentType = "application/octet-stream"; // Domyślny typ dla nieznanych rozszerzeń
            
            return contentType;
        }
    }
}
