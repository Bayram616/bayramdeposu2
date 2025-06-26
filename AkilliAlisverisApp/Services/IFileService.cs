using System.IO;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IFileService
    {
        Task<string> SaveStreamToFileAsync(Stream stream, string fileName);
        string GetContentType(string fileName);
    }
}