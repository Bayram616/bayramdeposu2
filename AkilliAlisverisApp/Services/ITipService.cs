using System.Collections.Generic;
using System.Threading.Tasks;
using AkilliAlisverisApp.Models;

namespace AkilliAlisverisApp.Services
{
    public interface ITipService
    {
        // GetLegalWarningTextAsync satırı buradan kaldırıldı.
        Task<List<TipCategory>> GetTipCategoriesAsync();
        Task<Tip?> PostTipAsync(Tip newTip);
        Task<bool> LikeTipAsync(int tipId);
        Task<bool> DislikeTipAsync(int tipId);
        Task<List<Tip>> GetApprovedTipsAsync();
        Task<Tip?> GetTipByIdAsync(int tipId);
    }
}