// AkilliAlisverisApp/ViewModels/ComplaintViewModel.cs

using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq; // LINQ kullanımı için eklendi
using System.Threading.Tasks;
using System.Windows.Input;

// NAMESPACE TANIMI BURADA BAŞLIYOR
namespace AkilliAlisverisApp.ViewModels
{
    // YARDIMCI SINIF ARTIK NAMESPACE İÇİNDE - DOĞRU YER
    public class ComplaintGroup : ObservableCollection<Complaint>
    {
        public string CategoryName { get; private set; }
        public ComplaintGroup(string categoryName, IEnumerable<Complaint> complaints) : base(complaints)
        {
            CategoryName = categoryName;
        }
    }

    public partial class ComplaintViewModel : BaseViewModel
    {
        private readonly IComplaintService _complaintService;

        [ObservableProperty]
        private ObservableCollection<ComplaintGroup> _complaintGroups;

        public ICommand NavigateToComplaintInsertCommand { get; }
        public ICommand NavigateToCategoryPageCommand { get; }

        public ComplaintViewModel(IComplaintService complaintService)
        {
            Title = "Şikayet ve Öneri";
            _complaintService = complaintService;
            ComplaintGroups = new ObservableCollection<ComplaintGroup>();

            NavigateToComplaintInsertCommand = new AsyncRelayCommand(NavigateToComplaintInsertAsync);
            NavigateToCategoryPageCommand = new AsyncRelayCommand<string>(NavigateToCategoryPageAsync);
        }

        [RelayCommand]
        public async Task LoadComplaintsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ComplaintGroups.Clear();

                var tasks = StaticData.ComplaintCategories.Select(async category => new
                {
                    CategoryName = category.Name,
                    Complaints = await _complaintService.GetComplaintsByCategoryAsync(category.Name)
                });

                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
                    if (result.Complaints.Any())
                    {
                        // BURADA HATA VEREN SATIR ŞİMDİ DOĞRU ÇALIŞACAK
                        var group = new ComplaintGroup(result.CategoryName, result.Complaints.Take(3));
                        ComplaintGroups.Add(group);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToComplaintInsertAsync()
        {
            await Shell.Current.GoToAsync(nameof(ComplaintInsertPage));
        }

        private async Task NavigateToCategoryPageAsync(string category)
        {
            if (string.IsNullOrEmpty(category)) return;

            // Diğer sayfaları sildiğimiz için bu navigasyon artık çalışmayacak.
            // Bu kısmı geçici olarak DisplayAlert ile değiştirebilir veya
            // ileride tek bir kategori detay sayfası yaparsak ona yönlendirebiliriz.
            // Şimdilik burayı yoruma alıyorum, ihtiyacımız olduğunda tekrar düzenleriz.
            /*
            string route = category switch
            {
                "Ürün Şikayetleri" => nameof(ProductComplaintsPage),
                "Marka Şikayetleri" => nameof(BrandComplaintsPage),
                "Satıcı Şikayetleri" => nameof(SellerComplaintsPage),
                "Diğer Şikayetler" => nameof(OtherComplaintsPage),
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(route))
            {
                await Shell.Current.GoToAsync(route);
            }
            */

            await Shell.Current.DisplayAlert("Detaylar", $"{category} kategorisindeki tüm şikayetler burada listelenecek.", "Tamam");
        }
    }
}