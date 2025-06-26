using Microsoft.Maui.Controls;
using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}