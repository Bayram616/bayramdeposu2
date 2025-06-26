using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views
{
    public partial class TipsView : ContentPage
    {
        private readonly TipsViewModel _viewModel;

        public TipsView(TipsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Sayfa her göründüðünde ipuçlarýný yükle
            await _viewModel.LoadTipsAsync();
        }
    }
}