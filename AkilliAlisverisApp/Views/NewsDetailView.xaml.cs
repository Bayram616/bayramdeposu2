using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views;

public partial class NewsDetailView : ContentPage
{
    public NewsDetailView(NewsDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}