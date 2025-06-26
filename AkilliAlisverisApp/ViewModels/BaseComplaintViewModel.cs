using AkilliAlisverisApp.Messages;
using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public abstract partial class BaseComplaintViewModel : BaseViewModel
    {
        private readonly IComplaintService _complaintService;
        private readonly WeakReferenceMessenger _messenger;

        [ObservableProperty]
        private ObservableCollection<Complaint> _complaints;

        public string ComplaintCategory { get; protected set; } // Kalıtım alan sınıflar tarafından set edilecek

        public IAsyncRelayCommand LoadComplaintsCommand { get; }

        protected BaseComplaintViewModel(IComplaintService complaintService, WeakReferenceMessenger messenger)
        {
            _complaintService = complaintService;
            _messenger = messenger;

            Complaints = new ObservableCollection<Complaint>();
            LoadComplaintsCommand = new AsyncRelayCommand(LoadComplaintsAsync);

            // Yeni şikayet eklendiğinde listeyi yenilemek için mesaj dinleyicisi
            _messenger.Register<ComplaintSubmittedMessage>(this, async (r, m) =>
            {
                if (m.IsSuccess)
                {
                    await LoadComplaintsAsync();
                }
            });
        }

        private async Task LoadComplaintsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Complaints.Clear();
                // ComplaintCategory, bu sınıftan türeyen ViewModel'de belirlenmiş olacak.
                var items = await _complaintService.GetComplaintsByCategoryAsync(ComplaintCategory);
                foreach (var item in items)
                {
                    Complaints.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Şikayetler yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToComplaintDetail(Complaint complaint)
        {
            // Bu metot tüm alt sınıflar için ortak olacak.
            if (complaint == null) return;
            await Shell.Current.DisplayAlert("Şikayet Detayı", $"Şikayet No: {complaint.Id}\nKonu: {complaint.Subject}\nİçerik: {complaint.Content}", "Tamam");
        }
    }
}