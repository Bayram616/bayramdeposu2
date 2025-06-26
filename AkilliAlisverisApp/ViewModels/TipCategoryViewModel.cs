using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AkilliAlisverisApp.ViewModels
{
    public class TipCategoryViewModel : BaseViewModel
    {
        private readonly ITipService _tipService;
        public ObservableCollection<TipCategory> TipCategories { get; } = new ObservableCollection<TipCategory>();
        public TipCategoryViewModel(ITipService tipService)
        {
            _tipService = tipService;
        }
        public async Task LoadTipCategoriesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var categories = await _tipService.GetTipCategoriesAsync();
                if (categories != null)
                {
                    TipCategories.Clear();
                    foreach (var category in categories)
                    {
                        TipCategories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Makale kategorileri yüklenirken hata oluştu: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}