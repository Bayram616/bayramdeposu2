using AkilliAlisverisApp.ViewModels.Reviews;

namespace AkilliAlisverisApp.Views.Reviews
{
    public partial class ProductReviewsPage : ContentPage
    {
        public ProductReviewsPage(ProductReviewViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
