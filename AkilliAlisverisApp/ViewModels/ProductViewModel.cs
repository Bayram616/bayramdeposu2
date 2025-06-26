using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public class ProductGroup : ObservableCollection<Product>
    {
        public string CategoryName { get; private set; }
        public ProductGroup(string categoryName, IEnumerable<Product> products) : base(products)
        {
            CategoryName = categoryName;
        }
    }
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private ObservableCollection<ProductGroup> _productGroups;

        public ProductViewModel(IProductService productService)
        {
            Title = "İndirimli Ürünler";
            _productService = productService;
            ProductGroups = new ObservableCollection<ProductGroup>();
        }

        [RelayCommand]
        private async Task LoadProductsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                ProductGroups.Clear();
                var allProducts = await _productService.GetAllProductsAsync();
                if (allProducts != null)
                {
                    var grouped = allProducts
                        .GroupBy(p => p.Category?.Name ?? "Diğer Ürünler")
                        .Select(g => new ProductGroup(g.Key, g.Take(5)));

                    var sortedGroups = StaticData.ProductCategories
                        .Select(catName => grouped.FirstOrDefault(g => g.CategoryName == catName))
                        .Where(g => g != null && g.Any());
                    foreach (var group in sortedGroups)
                    {
                        ProductGroups.Add(group);
                    }
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

        [RelayCommand]
        private async Task SearchProductsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return;
            await NavigateToProductCategoryAsync(searchText);
        }

        [RelayCommand]
        private async Task NavigateToProductCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category)) return;
            await Shell.Current.GoToAsync($"{nameof(ProductCategoryPage)}?categoryName={category}");
        }

        [RelayCommand]
        private async Task NavigateToProductDetailAsync(Product product)
        {
            if (product == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?productId={product.ProductID}");
        }
    }
}