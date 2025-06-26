using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views
{
    public partial class ProductDetailPage : ContentPage, IQueryAttributable
    {
        private readonly ProductDetailViewModel _viewModel;

        public ProductDetailPage(ProductDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await _viewModel.ApplyQueryAttributesAsync(query);
        }
    }
}
