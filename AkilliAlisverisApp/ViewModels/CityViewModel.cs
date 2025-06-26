using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public class CityViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public CityViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task LoadCitiesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // "GetListOfItemsFromApiAsync" yerine doğrudan "GetCitiesAsync" çağrılıyor
                var cities = await _apiService.GetCitiesAsync();

                if (cities != null)
                {
                    Cities.Clear();
                    foreach (var city in cities)
                        Cities.Add(city);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Şehirler yüklenirken hata: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}