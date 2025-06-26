using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json; // Eğer API JSON döndürseydi kullanılırdı

namespace AkilliAlisverisApp.Services
{
    public class SasService : ISasService
    {
        private readonly HttpClient _httpClient;

        public SasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSasTokenAsync(string fileName)
        {
            try
            {
                // API'nizin tam endpoint'i: BaseAddress + "Sas/GetSasToken?fileName={fileName}"
                var response = await _httpClient.GetAsync($"Sas/GetSasToken?fileName={fileName}");

                response.EnsureSuccessStatusCode(); // HTTP 2xx dışında bir durum varsa hata fırlatır

                var sasUrl = await response.Content.ReadAsStringAsync(); // API doğrudan SAS URL'sini string olarak döndürüyor

                return sasUrl;
            }
            catch (HttpRequestException ex)
            {
                // HTTP isteği sırasında oluşan hataları yakala
                Console.WriteLine($"HTTP Request Error in SasService: {ex.Message}");
                throw; // Hatayı ViewModel'e ilet
            }
            catch (Exception ex)
            {
                // Diğer genel hataları yakala
                Console.WriteLine($"Error in SasService: {ex.Message}");
                throw; // Hatayı ViewModel'e ilet
            }
        }
    }
}