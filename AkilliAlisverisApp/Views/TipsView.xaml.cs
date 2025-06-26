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
            // Sayfa her g�r�nd���nde ipu�lar�n� y�kle
            await _viewModel.LoadTipsAsync();
        }
    }
}