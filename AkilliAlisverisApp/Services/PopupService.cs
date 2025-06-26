using CommunityToolkit.Maui.Extensions;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Maui.Views;
using AkilliAlisverisApp.Services;

namespace AkilliAlisverisApp.Services
{
    public class PopupService : IAppPopupService
    {
        public async Task ShowLegalWarningAsync(string warningText)
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage is Page page)
            {
                await page.ShowPopupAsync(new LegalWarningPopup(warningText));
            }
        }
    }
}
