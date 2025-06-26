using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AkilliAlisverisApp.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class ProductCategoryViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IProductService _productService;

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                Title = value;
                OnPropertyChanged();
            }
        }

        private bool _hasLoaded;

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public ICommand LoadProductsCommand { get; }
        public ICommand TextChangedCommand { get; }
        public ICommand NavigateToProductDetailCommand { get; }

        public ProductCategoryViewModel(IProductService productService)
        {
            _productService = productService;
            LoadProductsCommand = new AsyncRelayCommand(LoadProductsAsync, () => !IsBusy);
            TextChangedCommand = new AsyncRelayCommand<string>(TextChangedAsync);
            NavigateToProductDetailCommand = new AsyncRelayCommand<Product>(NavigateToProductDetailAsync);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("categoryName", out var categoryObj) && categoryObj is string categoryName)
            {
                SelectedCategory = categoryName;
            }
            if (!_hasLoaded)
            {
                await LoadProductsAsync();
                _hasLoaded = true;
            }
        }

        private async Task LoadProductsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Products.Clear();
                var allProducts = await _productService.GetAllProductsAsync();
                var categoryProducts = allProducts
                    .Where(p => p.Category != null && p.Category.Name == SelectedCategory)
                    .ToList();
                foreach (var product in categoryProducts)
                {
                    Products.Add(product);
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Ürünler yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task TextChangedAsync(string searchText)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Products.Clear();
                var allProducts = await _productService.GetAllProductsAsync();
                var filteredProducts = allProducts
                    .Where(p => p.Category != null &&
                                p.Category.Name == SelectedCategory &&
                                (p.ProductName.ToLowerInvariant().Contains(searchText.ToLowerInvariant()) ||
                                 (!string.IsNullOrEmpty(p.Description) && p.Description.ToLowerInvariant().Contains(searchText.ToLowerInvariant())) ||
                                 (!string.IsNullOrEmpty(p.MarketName) && p.MarketName.ToLowerInvariant().Contains(searchText.ToLowerInvariant()))
                                ))
                    .ToList();
                foreach (var product in filteredProducts)
                {
                    Products.Add(product);
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Arama sırasında bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToProductDetailAsync(Product product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={product.ProductID}");
        }
    }
}