using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AkilliAlisverisApp.Helpers;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class ShoppingListViewModel : BaseViewModel
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IMarketService _marketService;

        [ObservableProperty]
        private ObservableCollection<ShoppingList> _shoppingLists;

        // Manuel olarak tanımlanan property
        private ObservableCollection<Market> _availableMarkets;
        public ObservableCollection<Market> AvailableMarkets
        {
            get => _availableMarkets;
            set => SetProperty(ref _availableMarkets, value);
        }

        public ShoppingListViewModel(IShoppingListService shoppingListService, IMarketService marketService)
        {
            _shoppingListService = shoppingListService;
            _marketService = marketService;
            ShoppingLists = new ObservableCollection<ShoppingList>();
            AvailableMarkets = new ObservableCollection<Market>();
            Title = "Alışveriş Listelerim";
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                int? userIdNullable = await SecureStorageHelper.GetUserIdAsync();
                if (userIdNullable == null || userIdNullable == 0)
                {
                    await Shell.Current.DisplayAlert("Giriş Gerekli", "Listelerinizi görmek için giriş yapmalısınız.", "Tamam");
                    return;
                }

                int userId = userIdNullable.Value;

                var shoppingListTask = _shoppingListService.GetShoppingListsByUserIdAsync(userId);
                var marketTask = _marketService.GetMarketsAsync();

                await Task.WhenAll(shoppingListTask, marketTask);

                var lists = await shoppingListTask;
                var markets = await marketTask;

                ShoppingLists.Clear();
                foreach (var list in lists)
                    ShoppingLists.Add(list);

                AvailableMarkets.Clear();
                foreach (var market in markets)
                    AvailableMarkets.Add(market);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Veri yükleme hatası: {ex.Message}");
                await Shell.Current.DisplayAlert("Hata", "Veriler yüklenirken bir sorun oluştu.", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToShoppingListItemsAsync(ShoppingList list)
        {
            if (list == null) return;
            await Shell.Current.GoToAsync($"{nameof(ShoppingListItemView)}?ListID={list.Id}");
        }

        [RelayCommand]
        private async Task CreateNewListAsync(Market selectedMarket)
        {
            if (selectedMarket == null)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir market seçin.", "Tamam");
                return;
            }

            int? userIdNullable = await SecureStorageHelper.GetUserIdAsync();
            if (userIdNullable == null || userIdNullable == 0)
            {
                await Shell.Current.DisplayAlert("Giriş Gerekli", "Liste oluşturmak için giriş yapmalısınız.", "Tamam");
                return;
            }

            int userId = userIdNullable.Value;

            var newList = new ShoppingList
            {
                UserID = userId,
                MarketID = selectedMarket.MarketID,
                ListName = selectedMarket.MarketName,
                CreatedDate = DateTime.UtcNow,
                IsCompleted = false
            };

            bool success = await _shoppingListService.CreateShoppingListAsync(newList);
            if (success)
            {
                await LoadDataAsync(); // Listeyi yenile
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Yeni liste oluşturulamadı.", "Tamam");
            }
        }
    }
}
