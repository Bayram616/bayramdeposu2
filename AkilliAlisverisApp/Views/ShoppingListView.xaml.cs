using AkilliAlisverisApp.ViewModels;
using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using System.Net.Http;
using System.Linq;

namespace AkilliAlisverisApp.Views
{
    public partial class ShoppingListView : ContentPage
    {
        private readonly ShoppingListViewModel _viewModel;

        public ShoppingListView(ShoppingListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadDataAsync(); // Veriler yükleniyor
        }
    }
}