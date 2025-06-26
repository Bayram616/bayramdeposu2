using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Models.DTOs; // Yeni DTO'lar için eklendi
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IUserService
    {
        // Dönüş tipi bool'dan AuthResponseDto'ya değiştirildi
        Task<AuthResponseDto?> RegisterUserAsync(User user);

        // Dönüş tipi User'dan AuthResponseDto'ya değiştirildi
        Task<AuthResponseDto?> LoginUserAsync(string email, string password);

        Task<User?> GetUserByIdAsync(int userId);
    }
}