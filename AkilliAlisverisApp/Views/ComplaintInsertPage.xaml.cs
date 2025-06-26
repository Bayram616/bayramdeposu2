using AkilliAlisverisApp.ViewModels;

namespace AkilliAlisverisApp.Views
{
    public partial class ComplaintInsertPage : ContentPage
    {
        public ComplaintInsertPage(ComplaintInsertViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Sayfa y�klendi�inde ViewModel'de herhangi bir �zel ba�lang�� i�lemi yap�lmas� gerekiyorsa
        // �rne�in, kategorileri API'den �ekmek gibi (�u an sabit listeden �ekiliyor)
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // E�er ViewModel'de bir LoadCommand varsa burada �a��rabilirsiniz
            // if (BindingContext is ComplaintInsertViewModel vm)
            // {
            //     await vm.LoadPageDataCommand.ExecuteAsync(null);
            // }
        }
    }
}