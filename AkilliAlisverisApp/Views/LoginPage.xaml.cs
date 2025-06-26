using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void OnLoginCompleted(object sender, System.EventArgs e)
        {
            if (BindingContext is LoginViewModel vm && vm.LoginCommand.CanExecute(null))
            {
                vm.LoginCommand.Execute(null);
            }
        }
    }
}
