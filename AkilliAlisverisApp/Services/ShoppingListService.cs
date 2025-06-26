using AkilliAlisverisApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly ApiService _apiService;

        public ShoppingListService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ShoppingList>> GetShoppingListsByUserIdAsync(int userId)
        {
            return await _apiService.GetShoppingListsByUserIdAsync(userId);
        }

        public async Task<ShoppingList?> GetShoppingListByIdAsync(int listId)
        {
            return await _apiService.GetShoppingListByIdAsync(listId);
        }

        public async Task<bool> CreateShoppingListAsync(ShoppingList newList)
        {
            return await _apiService.CreateShoppingListAsync(newList);
        }

        public async Task<bool> UpdateShoppingListAsync(ShoppingList updatedList)
        {
            return await _apiService.UpdateShoppingListAsync(updatedList);
        }

        public async Task<bool> DeleteShoppingListAsync(int listId)
        {
            return await _apiService.DeleteShoppingListAsync(listId);
        }

        public async Task<bool> AddProductToShoppingListAsync(int listId, ShoppingListItem item)
        {
            return await _apiService.AddProductToShoppingListAsync(listId, item);
        }

        public async Task<bool> UpdateShoppingListItemAsync(int listId, ShoppingListItem item)
        {
            return await _apiService.UpdateShoppingListItemAsync(listId, item);
        }

        public async Task<bool> DeleteShoppingListItemAsync(int listId, int itemId)
        {
            return await _apiService.DeleteShoppingListItemAsync(listId, itemId);
        }

        // Aşağıdaki metotlar artık ApiService üzerinden yönetilen API tabanlı IShopingListService arayüzünde olmadığı için kaldırılmıştır.
        // Eğer yerel ve API tabanlı listeleri aynı anda yönetmek isterseniz ayrı bir arayüz ve servis oluşturmanız gerekir.
        // Public Task<bool> AddProductToList(Product product) { throw new System.NotImplementedException(); }
        // Public Task<bool> RemoveProductFromList(int productId) { throw new System.NotImplementedException(); }
        // Public Task<ObservableCollection<Product>> GetShoppingList() { throw new System.NotImplementedException(); }
        // Public Task<bool> IsProductInList(int productId) { throw new System.NotImplementedException(); }
    }
}