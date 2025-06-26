using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface ITextService
    {
        Task<string> GetTextFromApiAsync(string textKey);
    }
}