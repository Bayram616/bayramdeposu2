using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AkilliAlisverisApp.Helpers;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private User currentUser;

        [ObservableProperty]
        private string title = "Profilim";

        public UserViewModel(IApiService apiService)
        {
            _apiService = apiService;
            LoadUserDataCommand = new AsyncRelayCommand(LoadUserDataAsync);
            LogoutCommand = new RelayCommand(Logout);
            UpdateInfoCommand = new RelayCommand(UpdateInfo, CanUpdateInfo);
        }

        public IAsyncRelayCommand LoadUserDataCommand { get; }
        public IRelayCommand LogoutCommand { get; }
        public IRelayCommand UpdateInfoCommand { get; }

        private bool CanUpdateInfo()
        {
            // Şu anda pasif (buton devre dışı). Gerektiğinde true yapabilirsiniz.
            return false;
        }

        public async Task LoadUserDataAsync()
        {
            try
            {
                var userId = await SecureStorageHelper.GetUserIdAsync();
                if (userId == null || userId == 0)
                {
                    await Shell.Current.DisplayAlert("Uyarı", "Kullanıcı bulunamadı. Lütfen giriş yapın.", "Tamam");
                    return;
                }

                var userFromApi = await _apiService.GetUserByIdAsync(userId.Value);


                if (userFromApi != null)
                {
                    CurrentUser = userFromApi;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Kullanıcı verileri alınamadı.", "Tamam");
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"Kullanıcı verisi yüklenirken hata: {ex.Message}");
                await Shell.Current.DisplayAlert("Hata", "Veri yüklenirken hata oluştu.", "Tamam");
            }
        }

        private void Logout()
        {
            // Kullanıcı çıkış işlemleri (örneğin SecureStorage temizleme vs)
            Helpers.SecureStorageHelper.RemoveUserId();
            // Uygulamada shell navigation root sayfası (örneğin LoginPage) olabilir:
            Shell.Current.GoToAsync("//login");
        }

        private void UpdateInfo()
        {
            // Güncelleme işlemleri için komut (şimdilik pasif)
            // Buton devre dışı bırakıldığı için çağrılmaz.
        }
    }
}
