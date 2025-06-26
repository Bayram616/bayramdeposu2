using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using AkilliAlisverisApp.Messages;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class LegalWarningPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private string legalWarningText;

        private readonly WeakReferenceMessenger _messenger;

        public LegalWarningPopupViewModel(string legalWarningText, WeakReferenceMessenger messenger)
        {
            LegalWarningText = legalWarningText;
            _messenger = messenger;
        }

        [RelayCommand]
        private void ClosePopup()
        {
            _messenger.Send(new ClosePopupMessage());
        }
    }
}
