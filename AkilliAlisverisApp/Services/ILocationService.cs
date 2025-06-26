using AkilliAlisverisApp.Models;

namespace AkilliAlisverisApp.Services
{
    public interface ILocationService
    {
        Task<List<City>> GetCitiesAsync();
        Task<List<District>> GetDistrictsByCityAsync(int cityId);
        Task<List<Neighborhood>> GetNeighborhoodsByDistrictAsync(int districtId);
    }
}