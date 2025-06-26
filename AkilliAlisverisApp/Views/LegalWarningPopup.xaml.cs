using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using AkilliAlisverisApp.Messages;
using System;

namespace AkilliAlisverisApp.Views
{
    public partial class LegalWarningPopup : Popup
    {
        private readonly WeakReferenceMessenger _messenger;

        public LegalWarningPopup(string legalText)
        {
            InitializeComponent();

            _messenger = WeakReferenceMessenger.Default;

            this.BindingContext = new ViewModels.LegalWarningPopupViewModel(legalText, _messenger);

            _messenger.Register<ClosePopupMessage>(this, async (r, m) =>
            {
                await this.CloseAsync();
            });

            this.Closed += OnPopupClosed;
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            _messenger.UnregisterAll(this);
        }
    }
}