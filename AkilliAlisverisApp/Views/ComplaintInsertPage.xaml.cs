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

        // Sayfa yüklendiðinde ViewModel'de herhangi bir özel baþlangýç iþlemi yapýlmasý gerekiyorsa
        // Örneðin, kategorileri API'den çekmek gibi (þu an sabit listeden çekiliyor)
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Eðer ViewModel'de bir LoadCommand varsa burada çaðýrabilirsiniz
            // if (BindingContext is ComplaintInsertViewModel vm)
            // {
            //     await vm.LoadPageDataCommand.ExecuteAsync(null);
            // }
        }
    }
}