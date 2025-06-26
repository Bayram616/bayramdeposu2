using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views
{
    public partial class UserView : ContentPage
    {
        public UserView(UserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
