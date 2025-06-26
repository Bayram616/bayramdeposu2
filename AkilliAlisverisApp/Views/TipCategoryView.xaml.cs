using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views;

public partial class TipCategoryView : ContentPage
{
    private readonly TipCategoryViewModel _viewModel;

    public TipCategoryView(TipCategoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Bu override metodu, sayfa her göründüðünde otomatik olarak çaðrýlýr.
    protected override async void OnAppearing()
    {
        base.OnAppearing(); // Base sýnýfýn OnAppearing metodunu çaðýrmak önemlidir.
        await _viewModel.LoadTipCategoriesAsync();
    }
}