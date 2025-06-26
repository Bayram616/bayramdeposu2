using System;
using System.IO;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public class FileService : IFileService
    {
        public async Task<string> SaveStreamToFileAsync(Stream stream, string fileName)
        {
            // Uygulamanın geçici dosya dizinini al
            string tempDir = FileSystem.CacheDirectory;
            string filePath = Path.Combine(tempDir, fileName);

            using (var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }
            return filePath;
        }

        public string GetContentType(string fileName)
        {
            // Basit bir MIME tipi eşleştirme. Gerçek uygulamalarda daha kapsamlı olabilir.
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                // Diğer dosya türleri eklenebilir
                _ => "application/octet-stream", // Varsayılan bilinmeyen tür
            };
        }
    }
}