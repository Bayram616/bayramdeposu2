using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views;

[QueryProperty(nameof(Category), "Category")]
public partial class ProductCategoryPage : ContentPage
{
    private readonly ProductCategoryViewModel _viewModel;

    public string Category
    {
        set => _viewModel.SelectedCategory = Uri.UnescapeDataString(value);
    }

    public ProductCategoryPage(ProductCategoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}