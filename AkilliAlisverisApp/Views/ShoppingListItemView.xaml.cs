using AkilliAlisverisApp.ViewModels;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views
{
    [QueryProperty(nameof(ListID), nameof(ListID))] // Sayfa düzeyinde QueryProperty
    public partial class ShoppingListItemView : ContentPage
    {
        private readonly ShoppingListItemViewModel _viewModel;

        public ShoppingListItemView(ShoppingListItemViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // QueryProperty tarafýndan set edilecek
        public int ListID
        {
            get => _viewModel.ListId;
            set
            {
                // ListID'yi ViewModel'e aktarýyoruz
                _viewModel.ListId = value;
                OnPropertyChanged(); // Bu property deðiþtiðinde UI'ý bilgilendir (çok önemli deðil burada)
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // ListID zaten QueryProperty tarafýndan set edilmiþ olacak.
            // ViewModel'daki LoadItemsAsync, ListID set edildiðinde otomatik çalýþacaktýr.
            // Eðer QueryProperty düzgün çalýþmazsa veya ViewModel'da doðrudan yükleme istemiyorsak,
            // burada _viewModel.LoadItemsAsync(ListID); çaðrýsý yapýlabilir.
        }
    }
}