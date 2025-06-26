using AkilliAlisverisApp.Helpers;
using AkilliAlisverisApp.Messages;
using AkilliAlisverisApp.ViewModels;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace AkilliAlisverisApp.Views
{
    public partial class TipsInsertView : ContentPage
    {
        private readonly WeakReferenceMessenger _messenger;

        public TipsInsertView(TipsInsertViewModel viewModel, WeakReferenceMessenger messenger)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _messenger = messenger;

            _messenger.Register<ShowPopupMessage>(this, async (r, m) =>
            {
                var popup = new LegalWarningPopup(m.Message);
                await this.ShowPopupAsync(popup, new PopupOptions
                {
                    PageOverlayColor = Colors.Black.WithAlpha(0.5f),
                    CanBeDismissedByTappingOutsideOfPopup = true
                });
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is TipsInsertViewModel vm)
                vm.LoadCategoriesCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messenger.UnregisterAll(this);
        }
    }
}
