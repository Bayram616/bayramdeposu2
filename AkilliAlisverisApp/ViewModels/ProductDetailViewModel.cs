using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using AkilliAlisverisApp.Helpers;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class ProductDetailViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IProductReviewService _productReviewService;
        private readonly IShoppingListService _shoppingListService;
        private readonly IMyNotificationService _myNotificationService;

        [ObservableProperty]
        private Product? currentProduct;

        [ObservableProperty]
        private ObservableCollection<ProductReview> productReviews = new();

        [ObservableProperty]
        private int userRating;

        [ObservableProperty]
        private string userComment = string.Empty;

        public ProductDetailViewModel(IProductService productService, IProductReviewService productReviewService, IShoppingListService shoppingListService, IMyNotificationService myNotificationService)
        {
            _productService = productService;
            _productReviewService = productReviewService;
            _shoppingListService = shoppingListService;
            _myNotificationService = myNotificationService;
        }

        public async Task ApplyQueryAttributesAsync(IDictionary<string, object> query)
        {
            if (query.TryGetValue("productId", out var productIdObj) && int.TryParse(productIdObj.ToString(), out int productId))
            {
                await LoadProductDetailAsync(productId);
            }
        }

        [RelayCommand]
        private async Task LoadProductDetailAsync(int productId)
        {
            if (IsBusy || productId == 0) return;
            IsBusy = true;
            try
            {
                CurrentProduct = await _productService.GetProductByIdAsync(productId);
                if (CurrentProduct != null)
                    await LoadReviewsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ürün yükleme hatası: {ex.Message}");
                await Shell.Current.DisplayAlert("Hata", "Ürün detayları yüklenemedi.", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadReviewsAsync()
        {
            ProductReviews.Clear();
            var reviews = await _productReviewService.GetReviewsByProductIdAsync(CurrentProduct!.ProductID);
            foreach (var review in reviews)
                ProductReviews.Add(review);
        }

        [RelayCommand]
        private async Task SubmitReviewAsync()
        {
            if (CurrentProduct == null || UserRating < 1 || UserRating > 5)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen 1-5 arası puan verin.", "Tamam");
                return;
            }

            int? userIdNullable = await SecureStorageHelper.GetUserIdAsync();
            if (userIdNullable == null)
            {
                await Shell.Current.DisplayAlert("Giriş Gerekli", "Yorum yapmak için giriş yapmalısınız.", "Tamam");
                return;
            }

            int userId = userIdNullable.Value;

            var newReview = new ProductReview
            {
                ProductId = CurrentProduct.ProductID,
                UserId = userId,
                Rating = UserRating,
                Comment = UserComment,
                CreatedDate = DateTime.Now
            };
            var result = await _productReviewService.SubmitReviewAsync(newReview);
            if (result != null)
            {
                UserRating = 0;
                UserComment = string.Empty;
                await LoadReviewsAsync();
                await Shell.Current.DisplayAlert("Teşekkürler", "Yorumunuz kaydedildi.", "Tamam");
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Yorum kaydedilemedi.", "Tamam");
            }
        }

        [RelayCommand]
        private async Task AddProductToShoppingListAsync(Product? product)
        {
            if (product == null || product.MarketID == 0)
            {
                await Shell.Current.DisplayAlert("Hata", "Ürün bilgisi veya market bilgisi eksik.", "Tamam");
                return;
            }

            int? userIdNullable = await SecureStorageHelper.GetUserIdAsync();
            if (userIdNullable == null)
            {
                await Shell.Current.DisplayAlert("Giriş Gerekli", "Ürünü listeye eklemek için giriş yapmalısınız.", "Tamam");
                return;
            }

            int userId = userIdNullable.Value;

            try
            {
                var allUserLists = await _shoppingListService.GetShoppingListsByUserIdAsync(userId);
                var targetList = allUserLists?.FirstOrDefault(l => l.MarketID == product.MarketID && !l.IsCompleted);

                if (targetList == null)
                {
                    var newList = new ShoppingList
                    {
                        UserID = userId,
                        MarketID = product.MarketID,
                        ListName = product.MarketName,
                        CreatedDate = DateTime.UtcNow,
                        IsCompleted = false
                    };
                    bool listCreated = await _shoppingListService.CreateShoppingListAsync(newList);
                    if (!listCreated)
                    {
                        await Shell.Current.DisplayAlert("Hata", "Yeni alışveriş listesi oluşturulamadı.", "Tamam");
                        return;
                    }

                    var updatedLists = await _shoppingListService.GetShoppingListsByUserIdAsync(userId);
                    targetList = updatedLists?.FirstOrDefault(l => l.MarketID == product.MarketID && !l.IsCompleted);
                }

                if (targetList == null)
                {
                    await Shell.Current.DisplayAlert("Hata", "Alışveriş listesi bulunamadı veya oluşturulamadı.", "Tamam");
                    return;
                }

                var newItem = new ShoppingListItem
                {
                    ListID = targetList.Id,
                    ProductID = product.ProductID,
                    CustomProductName = product.ProductName,
                    Quantity = 1,
                    IsPurchased = false
                };
                bool itemAdded = await _shoppingListService.AddProductToShoppingListAsync(targetList.Id, newItem);
                if (itemAdded)
                {
                    await Shell.Current.DisplayAlert("Başarılı", $"'{product.ProductName}' ürünü, {targetList.ListName} listenize eklendi.", "Tamam");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Ürün listeye eklenemedi.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddProductToShoppingListAsync Error: {ex.Message}");
                await Shell.Current.DisplayAlert("Kritik Hata", "Liste işlemi sırasında beklenmedik bir hata oluştu.", "Tamam");
            }
        }

        [RelayCommand]
        private async Task ScheduleProductNotificationAsync()
        {
            if (CurrentProduct == null) return;
            var granted = await _myNotificationService.RequestPermissions();
            if (!granted)
            {
                await Shell.Current.DisplayAlert("İzin Gerekli", "Bildirim için izin verin.", "Tamam");
                return;
            }

            var notifyTime = DateTime.Now.AddSeconds(10);
            var message = $"{CurrentProduct.ProductName} ürününü almayı unutmayın!";
            await _myNotificationService.ScheduleNotification("Hatırlatma", message, CurrentProduct.ProductID, notifyTime);
            await Shell.Current.DisplayAlert("Bildirim Planlandı", $"{notifyTime:HH:mm} için planlandı.", "Tamam");
        }
    }
}
