using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Models.DTOs;

namespace AkilliAlisverisApp.Services
{
    public interface IApiService
    {
        void AddAuthenticationHeader(string token);
        void RemoveAuthenticationHeader();

        Task<AuthResponseDto?> LoginUserAsync(string email, string password);
        Task<AuthResponseDto?> RegisterUserAsync(User user);
        Task<User?> GetUserByIdAsync(int userId);

        // Diğer ihtiyaç duyulan metotlar buraya eklenebilir
        Task<List<City>> GetCitiesAsync();
        Task<List<District>> GetDistrictsByCityAsync(int cityId);
        Task<List<Neighborhood>> GetNeighborhoodsByDistrictAsync(int districtId);
    }
}
