using System;
using System.Threading.Tasks;
using Plugin.LocalNotification; // Bu satır değişmedi

namespace AkilliAlisverisApp.Services
{
    public class NotificationService : IMyNotificationService // INotificationService yerine IMyNotificationService oldu
    {
        public NotificationService()
        {
            // Bildirim servisi başlatıldığında platforma özgü ayarlar burada yapılabilir
        }

        public async Task ScheduleNotification(string title, string message, int id, DateTime notifyTime)
        {
            var request = new NotificationRequest
            {
                NotificationId = id,
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    // Eğer bildirim tekrar etsin isterseniz bu kısmı kullanabilirsiniz.
                    // RepeatType = NotificationRepeat.Daily 
                }
            };

            var result = await LocalNotificationCenter.Current.Show(request);

            System.Diagnostics.Debug.WriteLine($"Bildirim Planlandı - ID: {id}, Başlık: '{title}', Mesaj: '{message}', Zaman: '{notifyTime}', Sonuç: {result}");
        }

        public void CancelNotification(int id)
        {
            LocalNotificationCenter.Current.Cancel(id);
            System.Diagnostics.Debug.WriteLine($"Bildirim İptal Edildi - ID: {id}");
        }

        public async Task<bool> RequestPermissions()
        {
            var granted = await LocalNotificationCenter.Current.RequestNotificationPermission();
            System.Diagnostics.Debug.WriteLine($"Bildirim İzin Durumu: {granted}");
            return granted;
        }
    }
}