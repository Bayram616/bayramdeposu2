using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views;

public partial class NewsView : ContentPage
{
    public NewsView(NewsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}