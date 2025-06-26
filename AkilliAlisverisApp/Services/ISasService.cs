namespace AkilliAlisverisApp.Services
{
    public interface ISasService
    {
        Task<string> GetSasTokenAsync(string fileName);
    }
}