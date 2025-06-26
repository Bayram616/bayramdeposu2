using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;

        [ObservableProperty]
        private ObservableCollection<NewsArticle> _articles;

        [ObservableProperty]
        private string _searchText;

        public NewsViewModel(INewsService newsService)
        {
            _newsService = newsService;
            Title = "Haberler";
            Articles = new ObservableCollection<NewsArticle>();
        }

        [RelayCommand]
        private async Task LoadNewsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Articles.Clear();
                var newsList = await _newsService.GetNewsArticlesAsync(SearchText);
                foreach (var article in newsList)
                {
                    Articles.Add(article);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetail(NewsArticle article)
        {
            if (article == null) return;
            // Detay sayfasına ID ile gideceğiz.
            await Shell.Current.GoToAsync($"{nameof(NewsDetailView)}?ArticleId={article.Id}");
        }
    }
}