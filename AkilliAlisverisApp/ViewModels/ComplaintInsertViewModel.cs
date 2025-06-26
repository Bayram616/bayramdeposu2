using AkilliAlisverisApp.Messages;
using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views; // LegalWarningPopup için
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls; // Shell.Current için
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class ComplaintInsertViewModel : BaseViewModel
    {
        private readonly IComplaintService _complaintService;
        private readonly ISasService _sasService;
        private readonly IFileService _fileService; // Dosya işlemlerini yönetecek servis
        private readonly IAppPopupService _popupService;
        private readonly WeakReferenceMessenger _messenger;

        [ObservableProperty]
        private string complaintSubject;

        [ObservableProperty]
        private string complaintContent;

        [ObservableProperty]
        private ObservableCollection<string> complaintCategories;

        [ObservableProperty]
        private string selectedCategory;

        [ObservableProperty]
        private ImageSource attachedImageSource; // Seçilen fotoğrafı göstermek için

        [ObservableProperty]
        private string attachedImageUrl; // Azure'a yüklendikten sonraki URL

        [ObservableProperty]
        private bool isLegalWarningAccepted;

        public ComplaintInsertViewModel(IComplaintService complaintService, ISasService sasService, IFileService fileService, WeakReferenceMessenger messenger)
        {
            _complaintService = complaintService;
            _sasService = sasService;
            _fileService = fileService;
            _messenger = messenger;

            Title = "Yeni Şikayet / Öneri";
            ComplaintCategories = new ObservableCollection<string>
            {
                "Ürün Şikayeti",
                "Mağaza/Hizmet Şikayeti",
                "Uygulama Önerisi",
                "Diğer"
            };

            // Varsayılan kategori seçimi
            SelectedCategory = ComplaintCategories.FirstOrDefault();
        }

        [RelayCommand]
        private async Task ShowLegalWarning()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var legalWarning = await _complaintService.GetLegalWarningTextAsync();

                await _popupService.ShowLegalWarningAsync(legalWarning);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Yasal uyarı metni yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectPhoto()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Lütfen bir fotoğraf seçin"
                });

                if (photo == null) return;

                // Fotoğrafı yerel olarak göster
                AttachedImageSource = ImageSource.FromStream(() => photo.OpenReadAsync().Result);

                // Fotoğrafı temp dosyasına kaydet ve yolunu al
                var stream = await photo.OpenReadAsync();
                var tempFilePath = await _fileService.SaveStreamToFileAsync(stream, photo.FileName);

                // Temp dosyasını kullanarak SAS URL'i al ve Azure'a yükle
                AttachedImageUrl = await UploadImageToAzure(tempFilePath, photo.FileName);

                if (!string.IsNullOrEmpty(AttachedImageUrl))
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Fotoğraf başarıyla yüklendi.", "Tamam");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Fotoğraf yüklenemedi.", "Tamam");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Shell.Current.DisplayAlert("Hata", $"Bu cihazda fotoğraf seçme özelliği desteklenmiyor: {fnsEx.Message}", "Tamam");
            }
            catch (PermissionException pEx)
            {
                await Shell.Current.DisplayAlert("Hata", $"Fotoğraf seçmek için izinler reddedildi. Lütfen uygulama ayarlarına giderek izinleri etkinleştirin: {pEx.Message}", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Fotoğraf seçilirken/yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task TakePhoto()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo == null) return;

                // Fotoğrafı yerel olarak göster
                AttachedImageSource = ImageSource.FromStream(() => photo.OpenReadAsync().Result);

                // Fotoğrafı temp dosyasına kaydet ve yolunu al
                var stream = await photo.OpenReadAsync();
                var tempFilePath = await _fileService.SaveStreamToFileAsync(stream, photo.FileName);

                // Temp dosyasını kullanarak SAS URL'i al ve Azure'a yükle
                AttachedImageUrl = await UploadImageToAzure(tempFilePath, photo.FileName);

                if (!string.IsNullOrEmpty(AttachedImageUrl))
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Fotoğraf başarıyla yüklendi.", "Tamam");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Fotoğraf yüklenemedi.", "Tamam");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Shell.Current.DisplayAlert("Hata", $"Bu cihazda fotoğraf çekme özelliği desteklenmiyor: {fnsEx.Message}", "Tamam");
            }
            catch (PermissionException pEx)
            {
                await Shell.Current.DisplayAlert("Hata", $"Fotoğraf çekmek için izinler reddedildi. Lütfen uygulama ayarlarına giderek izinleri etkinleştirin: {pEx.Message}", "Tamam");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Fotoğraf çekilirken/yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<string> UploadImageToAzure(string filePath, string originalFileName)
        {
            // Benzersiz bir dosya adı oluştur (örneğin: sikayet_GUID.jpg)
            string uniqueFileName = $"sikayet_{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";

            try
            {
                // Adım 1: API'den SAS URL'ini al
                var sasUrl = await _sasService.GetSasTokenAsync(uniqueFileName);
                if (string.IsNullOrEmpty(sasUrl))
                {
                    Console.WriteLine("SAS URL alınamadı.");
                    return null;
                }
                Console.WriteLine($"SAS URL alındı: {sasUrl}");

                // Adım 2: Resmi doğrudan Azure Blob Storage'a yükle (SAS URL kullanarak)
                using (var httpClient = new HttpClient())
                using (var fileStream = File.OpenRead(filePath))
                {
                    var content = new StreamContent(fileStream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_fileService.GetContentType(uniqueFileName)); // MIME tipi

                    var response = await httpClient.PutAsync(sasUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Fotoğraf Azure Blob Storage'a başarıyla yüklendi.");
                        // Başarılı yüklemeden sonra SAS token'sız URL'yi kaydet
                        // SAS URL'den token kısmını çıkararak temiz URL'yi alıyoruz
                        var uriBuilder = new UriBuilder(sasUrl);
                        uriBuilder.Query = null; // Query string'i (SAS token'ı) kaldır
                        return uriBuilder.Uri.ToString(); // Temiz URL
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Azure'a yüklenirken hata oluştu: {response.StatusCode} - {errorContent}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Resim Azure'a yüklenirken genel bir hata oluştu: {ex.Message}");
                return null;
            }
            finally
            {
                // Geçici dosyayı sil
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [RelayCommand]
        private async Task SubmitComplaint()
        {
            if (IsBusy) return;

            // Validasyonlar
            if (string.IsNullOrWhiteSpace(ComplaintSubject))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir şikayet konusu giriniz.", "Tamam");
                return;
            }
            if (string.IsNullOrWhiteSpace(ComplaintContent))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen şikayetinizin içeriğini giriniz.", "Tamam");
                return;
            }
            if (string.IsNullOrWhiteSpace(SelectedCategory))
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen bir kategori seçiniz.", "Tamam");
                return;
            }
            if (!IsLegalWarningAccepted)
            {
                await Shell.Current.DisplayAlert("Uyarı", "Lütfen yasal uyarıyı okuyup kabul ediniz.", "Tamam");
                return;
            }

            IsBusy = true;
            try
            {
                // Kullanıcı adını al. Bu örnek için sabit bir değer kullandım.
                // Gerçek uygulamada, oturum açmış kullanıcının adını/ID'sini AuthenticationService'den almalısınız.
                string currentUserName = "AnonimKullanıcı"; // TODO: Oturum açmış kullanıcının adını al

                var newComplaint = new Complaint
                {
                    Subject = ComplaintSubject,
                    Content = ComplaintContent,
                    Category = SelectedCategory,
                    ImageUrl = AttachedImageUrl, // Azure'a yüklenen resmin URL'si
                    Date = DateTime.UtcNow,
                    Status = "Yeni", // Varsayılan durum
                    UserName = currentUserName
                };

                var success = await _complaintService.SubmitComplaintAsync(newComplaint);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Başarılı", "Şikayetiniz/Öneriniz başarıyla gönderildi!", "Tamam");
                    _messenger.Send(new ComplaintSubmittedMessage(true, "Şikayet başarıyla gönderildi."));
                    // Sayfadan geri git veya formu temizle
                    await Shell.Current.GoToAsync(".."); // Önceki sayfaya dön
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hata", "Şikayetiniz/Öneriniz gönderilirken bir hata oluştu. Lütfen tekrar deneyin.", "Tamam");
                    _messenger.Send(new ComplaintSubmittedMessage(false, "Şikayet gönderilirken hata oluştu."));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Hata", $"Şikayet gönderilirken beklenmedik bir hata oluştu: {ex.Message}", "Tamam");
                _messenger.Send(new ComplaintSubmittedMessage(false, $"Beklenmedik hata: {ex.Message}"));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}