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

    // Bu override metodu, sayfa her g�r�nd���nde otomatik olarak �a�r�l�r.
    protected override async void OnAppearing()
    {
        base.OnAppearing(); // Base s�n�f�n OnAppearing metodunu �a��rmak �nemlidir.
        await _viewModel.LoadTipCategoriesAsync();
    }
}