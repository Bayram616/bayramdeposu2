using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AkilliAlisverisApp.Views.Reviews;

namespace AkilliAlisverisApp.ViewModels.Reviews
{
    public partial class ProductReviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProductReview> productReviews = new();

        public ProductReviewViewModel(IProductReviewService reviewService) => _reviewService = reviewService;

        private readonly IProductReviewService _reviewService;

        public async Task LoadReviews(int productId)
        {
            var list = await _reviewService.GetReviewsByProductIdAsync(productId);
            ProductReviews.Clear();
            foreach (var item in list)
                ProductReviews.Add(item);
        }
    }
}
