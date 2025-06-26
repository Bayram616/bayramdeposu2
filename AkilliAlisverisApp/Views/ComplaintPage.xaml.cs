using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views;

public partial class ComplaintPage : ContentPage
{
    private readonly ComplaintViewModel _viewModel;

    public ComplaintPage(ComplaintViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}