// AkilliAlisverisApp.Services.IMyNotificationService.cs
using System;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IMyNotificationService // İsim INotificationService yerine IMyNotificationService oldu
    {
        Task ScheduleNotification(string title, string message, int id, DateTime notifyTime);
        void CancelNotification(int id);
        Task<bool> RequestPermissions();
    }
}