using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Helpers
{
    public static class SecureStorageHelper
    {
        public static async Task SetUserIdAsync(int userId) =>
            await SecureStorage.SetAsync("UserId", userId.ToString());

        public static async Task<int?> GetUserIdAsync()
        {
            var value = await SecureStorage.GetAsync("UserId");
            return int.TryParse(value, out var result) ? result : null;
        }

        public static async Task SetTokenAsync(string token) =>
            await SecureStorage.SetAsync("Token", token);

        public static async Task<string?> GetTokenAsync() =>
            await SecureStorage.GetAsync("Token");

        public static async Task ClearAllAsync()
        {
            SecureStorage.Remove("UserId");
            SecureStorage.Remove("Token");
        }
        public static void RemoveUserId()
        {
            SecureStorage.Remove("UserID");
        }
    }
}
