using AkilliAlisverisApp.Helpers;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        public LoginViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            var authResult = await _apiService.LoginUserAsync(Email, Password);

            if (authResult != null && authResult.UserId > 0)
            {
                await SecureStorageHelper.SetUserIdAsync(authResult.UserId);
                _apiService.AddAuthenticationHeader(authResult.Token);
                await Shell.Current.GoToAsync("//profile"); // AppShell içinde bu route tanımlı olmalı
            }
            else
            {
                ErrorMessage = "Giriş başarısız. Lütfen e-posta ve şifrenizi kontrol edin.";
            }

            IsBusy = false;
        }
    }
}
