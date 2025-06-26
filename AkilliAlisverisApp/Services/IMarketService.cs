// AkilliAlisverisApp.Services.IMarketService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using AkilliAlisverisApp.Models; // Market, MarketLogoDto, MarketSelectionDto için gerekli

namespace AkilliAlisverisApp.Services
{
    public interface IMarketService
    {
        Task<List<Market>> GetMarketsAsync();
        Task<List<MarketLogoDto>> GetMarketLogosByNeighborhoodAsync(int neighborhoodId);
        Task<List<MarketSelectionDto>> GetMarketSelectionsByNeighborhoodAsync(int neighborhoodId);
    }
}