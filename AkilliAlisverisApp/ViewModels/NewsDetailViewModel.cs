using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    // Sayfaya navigasyonla gelen parametreyi almak için IQueryAttributable implemente ediyoruz.
    public partial class NewsDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly INewsService _newsService;
        private int _articleId;

        [ObservableProperty]
        private NewsArticle _currentArticle = new(); // Sayfa yüklenirken null hatası almamak için başlatıyoruz.

        [ObservableProperty]
        private ObservableCollection<NewsReview> _reviews = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitReviewCommand))]
        private int _newUserRating;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitReviewCommand))]
        private string _newUserComment = string.Empty;

        public NewsDetailViewModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        // Navigasyonla gelen "ArticleId" parametresini burada yakalıyoruz.
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("ArticleId", out var idObject) && int.TryParse(idObject.ToString(), out var id))
            {
                _articleId = id;
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var articleTask = _newsService.GetNewsArticleByIdAsync(_articleId);
                var reviewsTask = _newsService.GetReviewsForArticleAsync(_articleId);

                await Task.WhenAll(articleTask, reviewsTask);

                CurrentArticle = await articleTask;

                Reviews.Clear();
                var reviewList = await reviewsTask;
                foreach (var review in reviewList)
                {
                    Reviews.Add(review);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LikeArticleAsync()
        {
            if (await _newsService.LikeArticleAsync(_articleId))
            {
                CurrentArticle.LikeCount++;
            }
        }

        [RelayCommand]
        private async Task DislikeArticleAsync()
        {
            if (await _newsService.DislikeArticleAsync(_articleId))
            {
                CurrentArticle.DislikeCount++;
            }
        }

        private bool CanSubmitReview()
        {
            return NewUserRating > 0 && NewUserRating <= 5 && !string.IsNullOrWhiteSpace(NewUserComment);
        }

        [RelayCommand(CanExecute = nameof(CanSubmitReview))]
        private async Task SubmitReviewAsync()
        {
            var result = await _newsService.SubmitReviewAsync(_articleId, NewUserRating, NewUserComment);
            if (result != null)
            {
                Reviews.Add(result); // Yeni yorumu listeye ekle
                // Inputları temizle
                NewUserComment = string.Empty;
                NewUserRating = 0;
            }
            else
            {
                await Shell.Current.DisplayAlert("Hata", "Yorum gönderilemedi.", "Tamam");
            }
        }
    }
}