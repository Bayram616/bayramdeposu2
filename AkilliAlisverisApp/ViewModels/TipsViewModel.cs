using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using AkilliAlisverisApp.Views;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class TipsViewModel : BaseViewModel
    {
        private readonly ITipService _tipService;
        public ObservableCollection<Tip> Tips { get; } = new ObservableCollection<Tip>();

        public TipsViewModel(ITipService tipService)
        {
            _tipService = tipService;
        }

        [RelayCommand]
        public async Task LoadTipsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var tips = await _tipService.GetApprovedTipsAsync();
                if (tips != null)
                {
                    Tips.Clear();
                    foreach (var tip in tips)
                    {
                        Tips.Add(tip);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"İpuçları yüklenirken hata oluştu: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToInsertTipAsync()
        {
            await Shell.Current.GoToAsync(nameof(TipsInsertView));
        }

        [RelayCommand]
        private async Task LikeTipAsync(Tip tip)
        {
            if (tip == null) return;
            bool success = await _tipService.LikeTipAsync(tip.TipID);
            if (success)
            {
                tip.LikeCount++;
            }
        }

        [RelayCommand]
        private async Task DislikeTipAsync(Tip tip)
        {
            if (tip == null) return;
            bool success = await _tipService.DislikeTipAsync(tip.TipID);
            if (success)
            {
                tip.DislikeCount++;
            }
        }

        [RelayCommand]
        private async Task ShowCommentsAsync(Tip tip)
        {
            if (tip == null) return;
            await Shell.Current.DisplayAlert("Yorumlar", "Yorumlar özelliği yakında eklenecek.", "Tamam");
        }
    }
}