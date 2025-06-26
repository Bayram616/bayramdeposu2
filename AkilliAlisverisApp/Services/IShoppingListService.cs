using AkilliAlisverisApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IShoppingListService
    {
        Task<List<ShoppingList>> GetShoppingListsByUserIdAsync(int userId);
        Task<ShoppingList?> GetShoppingListByIdAsync(int listId);
        Task<bool> CreateShoppingListAsync(ShoppingList newList);
        Task<bool> UpdateShoppingListAsync(ShoppingList updatedList);
        Task<bool> DeleteShoppingListAsync(int listId);
        Task<bool> AddProductToShoppingListAsync(int listId, ShoppingListItem item); // Product yerine ShoppingListItem almalı
        Task<bool> UpdateShoppingListItemAsync(int listId, ShoppingListItem item);
        Task<bool> DeleteShoppingListItemAsync(int listId, int itemId);
    }
}