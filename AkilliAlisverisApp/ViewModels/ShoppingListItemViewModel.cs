using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class ShoppingListItemViewModel : BaseViewModel
    {
        private readonly IShoppingListService _shoppingListService;

        [ObservableProperty]
        private int _listId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private string _newItemName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private int _quantity = 1;

        [ObservableProperty]
        private string _note = string.Empty;

        public ObservableCollection<ShoppingListItem> ShoppingListItems { get; } = new();

        public ShoppingListItemViewModel(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [RelayCommand]
        public async Task LoadItemsAsync()
        {
            if (IsBusy || ListId == 0) return;
            IsBusy = true;
            try
            {
                ShoppingListItems.Clear();
                var listWithItems = await _shoppingListService.GetShoppingListByIdAsync(ListId);
                if (listWithItems?.ShoppingListItems != null)
                {
                    foreach (var item in listWithItems.ShoppingListItems)
                    {
                        ShoppingListItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading shopping list items: {ex.Message}");
                await Shell.Current.DisplayAlert("Hata", "Liste öğeleri yüklenirken bir sorun oluştu.", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanAddItem()
        {
            return ListId > 0 && !string.IsNullOrWhiteSpace(NewItemName) && Quantity > 0;
        }

        [RelayCommand(CanExecute = nameof(CanAddItem))]
        private async Task AddItemAsync()
        {
            var newItem = new ShoppingListItem
            {
                ListID = ListId,
                CustomProductName = NewItemName,
                Quantity = Quantity,
                Note = Note
            };

            var success = await _shoppingListService.AddProductToShoppingListAsync(ListId, newItem);
            if (success)
            {
                await LoadItemsAsync(); // Listeyi yenile
                NewItemName = string.Empty;
                Quantity = 1;
                Note = string.Empty;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Ürün listeye eklenirken bir sorun oluştu.", "Tamam");
            }
        }

        [RelayCommand]
        private async Task DeleteItemAsync(ShoppingListItem item)
        {
            if (item == null || item.Id == 0) return;
            bool confirm = await Shell.Current.DisplayAlert("Onayla", $"'{item.CustomProductName}' öğesini listeden silmek istediğinizden emin misiniz?", "Evet", "Hayır");
            if (!confirm) return;

            var success = await _shoppingListService.DeleteShoppingListItemAsync(ListId, item.Id);
            if (success)
            {
                ShoppingListItems.Remove(item);
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Öğe silinirken bir sorun oluştu.", "Tamam");
            }
        }
    }
}