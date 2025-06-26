using System.Collections.Generic;
using System.Threading.Tasks;
using AkilliAlisverisApp.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AkilliAlisverisApp.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly HttpClient _httpClient;

        public ComplaintService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Complaint>> GetComplaintsByCategoryAsync(string category)
        {
            try
            {
                // API'den kategoriye göre şikayetleri çekme
                // Örn: GET api/Complaints?category={category}
                var response = await _httpClient.GetAsync($"Complaints?category={Uri.EscapeDataString(category)}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var complaints = JsonSerializer.Deserialize<List<Complaint>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return complaints ?? new List<Complaint>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting complaints by category {category}: {ex.Message}");
                return new List<Complaint>();
            }
        }

        public async Task<bool> SubmitComplaintAsync(Complaint complaint)
        {
            try
            {
                // Şikayeti API'ye POST etme
                // Örn: POST api/Complaints
                var json = JsonSerializer.Serialize(complaint);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Complaints", httpContent);

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting complaint: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetLegalWarningTextAsync()
        {
            try
            {
                // API'den yasal uyarı metnini çekme
                // Örn: GET api/LegalWarning/GetText
                var response = await _httpClient.GetAsync("LegalWarning/GetText");
                response.EnsureSuccessStatusCode();

                var legalText = await response.Content.ReadAsStringAsync();
                return legalText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting legal warning text: {ex.Message}");
                return "Yasal uyarı metni yüklenemedi. Lütfen daha sonra tekrar deneyin.";
            }
        }
    }
}