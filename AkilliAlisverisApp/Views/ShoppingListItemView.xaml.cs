using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views
{
    [QueryProperty(nameof(ListID), nameof(ListID))] // Sayfa d�zeyinde QueryProperty
    public partial class ShoppingListItemView : ContentPage
    {
        private readonly ShoppingListItemViewModel _viewModel;

        public ShoppingListItemView(ShoppingListItemViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // QueryProperty taraf�ndan set edilecek
        public int ListID
        {
            get => _viewModel.ListId;
            set
            {
                // ListID'yi ViewModel'e aktar�yoruz
                _viewModel.ListId = value;
                OnPropertyChanged(); // Bu property de�i�ti�inde UI'� bilgilendir (�ok �nemli de�il burada)
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // ListID zaten QueryProperty taraf�ndan set edilmi� olacak.
            // ViewModel'daki LoadItemsAsync, ListID set edildi�inde otomatik �al��acakt�r.
            // E�er QueryProperty d�zg�n �al��mazsa veya ViewModel'da do�rudan y�kleme istemiyorsak,
            // burada _viewModel.LoadItemsAsync(ListID); �a�r�s� yap�labilir.
        }
    }
}