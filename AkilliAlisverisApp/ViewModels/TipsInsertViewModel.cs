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
    public partial class TipsInsertViewModel : BaseViewModel
    {
        private readonly ITipService _tipService;
        private readonly ITextService _textService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitTipCommand))]
        private string _tipTitle = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitTipCommand))]
        private string _tipContent = string.Empty;

        [ObservableProperty]
        private ObservableCollection<TipCategory> _tipCategories = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitTipCommand))]
        private TipCategory? _selectedTipCategory;

        public TipsInsertViewModel(ITipService tipService, ITextService textService)
        {
            _tipService = tipService;
            _textService = textService;
            Title = "Yeni İpucu Ekle";
        }

        [RelayCommand]
        private async Task LoadCategoriesAsync()
        {
            if (IsBusy || TipCategories.Any()) return;
            IsBusy = true;
            try
            {
                var categories = await _tipService.GetTipCategoriesAsync();
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        TipCategories.Add(category);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSubmitTip()
        {
            return !string.IsNullOrWhiteSpace(TipTitle) &&
                   !string.IsNullOrWhiteSpace(TipContent) &&
                   SelectedTipCategory != null &&
                   !IsBusy;
        }

        [RelayCommand(CanExecute = nameof(CanSubmitTip))]
        private async Task SubmitTipAsync()
        {
            IsBusy = true;
            try
            {
                var newTip = new Tip
                {
                    Title = TipTitle,
                    Content = TipContent,
                    CategoryID = SelectedTipCategory?.CategoryID,
                };

                var result = await _tipService.PostTipAsync(newTip);
                if (result != null)
                {
                    await Shell.Current.DisplayAlert("Başarılı", "İpucunuz gönderildi ve onaya alındı.", "Tamam");
                    await Shell.Current.GoToAsync(".."); // Bir önceki sayfaya git
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "İpucu gönderilirken bir hata oluştu.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"İpucu gönderme hatası: {ex.Message}");
                await Shell.Current.DisplayAlert("Hata", "Beklenmedik bir hata oluştu.", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}