using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly ITextService _textService;

        public RegisterViewModel(ILocationService locationService, IUserService userService, ITextService textService)
        {
            _locationService = locationService;
            _userService = userService;
            _textService = textService;
            Title = "Kayıt Ol";
            _ = LoadCitiesAsync();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private string _userName = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private string _email = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private string _fullName = string.Empty;

        [ObservableProperty]
        private string _phoneNumber = string.Empty;

        [ObservableProperty]
        private DateTime _birthDate = DateTime.Today;

        [ObservableProperty]
        private string _gender = string.Empty;

        [ObservableProperty]
        private City? _selectedCity;

        [ObservableProperty]
        private District? _selectedDistrict;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private Neighborhood? _selectedNeighborhood;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private bool _isMembershipAgreementAccepted;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRegisterEnabled))]
        private bool _isKVKKAgreementAccepted;

        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
        public ObservableCollection<District> Districts { get; } = new ObservableCollection<District>();
        public ObservableCollection<Neighborhood> Neighborhoods { get; } = new ObservableCollection<Neighborhood>();
        public ReadOnlyCollection<string> Genders { get; } = new ReadOnlyCollection<string>(new List<string> { "Erkek", "Kadın", "Belirtmek İstemiyorum" });

        public bool IsRegisterEnabled => CanRegister();

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password == ConfirmPassword &&
                   !string.IsNullOrWhiteSpace(FullName) &&
                   SelectedNeighborhood != null &&
                   IsMembershipAgreementAccepted &&
                   IsKVKKAgreementAccepted &&
                   !IsBusy;
        }

        [RelayCommand(CanExecute = nameof(CanRegister))]
        private async Task RegisterAsync()
        {
            IsBusy = true;
            try
            {
                var newUser = new User
                {
                    UserName = UserName,
                    Email = Email,
                    PasswordHash = Password,
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    BirthDate = BirthDate,
                    Gender = Gender,
                    CityId = SelectedCity?.CityId,
                    DistrictId = SelectedDistrict?.DistrictId,
                    NeighborhoodId = SelectedNeighborhood!.NeighborhoodId,
                    CreatedAt = DateTime.UtcNow
                };

                var authResponse = await _userService.RegisterUserAsync(newUser);
                if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Kayıt işlemi tamamlandı. Giriş sayfasına yönlendiriliyorsunuz.", "Tamam");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Kayıt sırasında bir hata oluştu.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Beklenmeyen bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ShowMembershipAgreementAsync()
        {
            var text = await _textService.GetTextFromApiAsync("UyelikSozlesmesi");
            await Shell.Current.DisplayAlert("Üyelik Sözleşmesi", text ?? "Metin bulunamadı.", "Tamam");
        }

        [RelayCommand]
        private async Task ShowKVKKAgreementAsync()
        {
            var text = await _textService.GetTextFromApiAsync("KVKKMetni");
            await Shell.Current.DisplayAlert("KVKK Aydınlatma Metni", text ?? "Metin bulunamadı.", "Tamam");
        }

        private async Task LoadCitiesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var cities = await _locationService.GetCitiesAsync();
                if (cities != null && cities.Any())
                {
                    Cities.Clear();
                    foreach (var city in cities)
                    {
                        Cities.Add(city);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Şehirler yüklenirken hata: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnSelectedCityChanged(City? value)
        {
            Districts.Clear();
            Neighborhoods.Clear();
            SelectedDistrict = null;
            SelectedNeighborhood = null;
            if (value != null)
            {
                _ = LoadDistrictsAsync(value.CityId);
            }
        }

        private async Task LoadDistrictsAsync(int cityId)
        {
            var districts = await _locationService.GetDistrictsByCityAsync(cityId);
            if (districts != null && districts.Any())
            {
                foreach (var district in districts)
                {
                    Districts.Add(district);
                }
            }
        }

        partial void OnSelectedDistrictChanged(District? value)
        {
            Neighborhoods.Clear();
            SelectedNeighborhood = null;
            if (value != null)
            {
                _ = LoadNeighborhoodsAsync(value.DistrictId);
            }
        }

        private async Task LoadNeighborhoodsAsync(int districtId)
        {
            var neighborhoods = await _locationService.GetNeighborhoodsByDistrictAsync(districtId);
            if (neighborhoods != null && neighborhoods.Any())
            {
                foreach (var neighborhood in neighborhoods)
                {
                    Neighborhoods.Add(neighborhood);
                }
            }
        }
    }
}