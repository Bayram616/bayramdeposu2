using AkilliAlisverisApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface INewsService
    {
        Task<List<NewsArticle>> GetNewsArticlesAsync(string? searchText = null);
        Task<NewsArticle> GetNewsArticleByIdAsync(int articleId);
        Task<List<NewsReview>> GetReviewsForArticleAsync(int articleId);
        Task<bool> LikeArticleAsync(int articleId);
        Task<bool> DislikeArticleAsync(int articleId);
        Task<NewsReview> SubmitReviewAsync(int articleId, int rating, string comment);
    }
}