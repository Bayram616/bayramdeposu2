using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views;

public partial class ProductView : ContentPage
{
    private readonly ProductViewModel _viewModel;

    public ProductView(ProductViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}