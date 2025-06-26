using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views;

public partial class CityView : ContentPage
{
    public CityView(CityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}