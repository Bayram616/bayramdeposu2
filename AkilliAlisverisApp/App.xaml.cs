using AkilliAlisverisApp.Helpers;
using AkilliAlisverisApp.Services;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Giriş yapılmışsa token'ı geri ekle
            var token = await SecureStorageHelper.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                var apiService = ServiceHelper.GetService<IApiService>();
                apiService?.AddAuthenticationHeader(token);
            }
        }
    }
}
