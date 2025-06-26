using AkilliAlisverisApp.Helpers;
using AkilliAlisverisApp.Models;
using AkilliAlisverisApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public class ApiService : IApiService, ILocationService, IUserService, IMarketService, ITipService, IProductService, INewsService, IShoppingListService, IProductReviewService, ITextService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions);
                }
                Debug.WriteLine($"--> API GET Error on {endpoint}: {response.StatusCode}");
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API GET Exception on {endpoint}: {ex.Message}");
                return default;
            }
        }

        private async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonSerializerOptions);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentLength == 0) return default;
                    return await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions);
                }
                Debug.WriteLine($"--> API POST Error on {endpoint}: {response.StatusCode} | Content: {await response.Content.ReadAsStringAsync()}");
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API POST Exception on {endpoint}: {ex.Message}");
                return default;
            }
        }

        private async Task<bool> PostReturnBoolAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonSerializerOptions);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API POST (bool) Exception on {endpoint}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> PostAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.PostAsync(endpoint, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API POST (no content) Exception on {endpoint}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> PutAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonSerializerOptions);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API PUT Exception on {endpoint}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--> API DELETE Exception on {endpoint}: {ex.Message}");
                return false;
            }
        }

        public void AddAuthenticationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void RemoveAuthenticationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<List<City>> GetCitiesAsync() => await GetAsync<List<City>>("location/cities") ?? new();
        public async Task<List<District>> GetDistrictsByCityAsync(int cityId) => await GetAsync<List<District>>($"location/districts/{cityId}") ?? new();
        public async Task<List<Neighborhood>> GetNeighborhoodsByDistrictAsync(int districtId) => await GetAsync<List<Neighborhood>>($"location/neighborhoods/{districtId}") ?? new();

        public async Task<AuthResponseDto?> LoginUserAsync(string email, string password)
        {
            var request = new LoginRequestDto { Email = email, Password = password };
            var authResponse = await PostAsync<LoginRequestDto, AuthResponseDto>("auth/login", request);
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                AddAuthenticationHeader(authResponse.Token);
                await SecureStorage.Default.SetAsync("auth_token", authResponse.Token);
                await SecureStorageHelper.SetUserIdAsync(authResponse.UserId);
            }
            return authResponse;
        }

        public async Task<AuthResponseDto?> RegisterUserAsync(User user)
        {
            var request = new RegisterRequestDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                CityId = user.CityId,
                DistrictId = user.DistrictId,
                NeighborhoodId = user.NeighborhoodId
            };
            return await PostAsync<RegisterRequestDto, AuthResponseDto>("auth/register", request);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var userDetail = await GetAsync<UserDetailDto>($"users/{userId}");
            if (userDetail == null) return null;

            return new User
            {
                UserID = userDetail.UserID,
                UserName = userDetail.UserName,
                Email = userDetail.Email,
                FullName = userDetail.FullName,
                PhoneNumber = userDetail.PhoneNumber,
                BirthDate = userDetail.BirthDate,
                Gender = userDetail.Gender,
                City = userDetail.City != null ? new City { CityName = userDetail.City.CityName } : null,
                District = userDetail.District != null ? new District { DistrictName = userDetail.District.DistrictName } : null,
                Neighborhood = userDetail.Neighborhood != null ? new Neighborhood { NeighborhoodName = userDetail.Neighborhood.NeighborhoodName } : null
            };
        }

        public async Task<List<Market>> GetMarketsAsync() => await GetAsync<List<Market>>("markets") ?? new();
        public async Task<List<MarketLogoDto>> GetMarketLogosByNeighborhoodAsync(int neighborhoodId)
        {
            return await GetAsync<List<MarketLogoDto>>($"markets/by-neighborhood/{neighborhoodId}/logos") ?? new();
        }
        public async Task<List<MarketSelectionDto>> GetMarketSelectionsByNeighborhoodAsync(int neighborhoodId)
        {
            return await GetAsync<List<MarketSelectionDto>>($"markets/by-neighborhood/{neighborhoodId}/selections") ?? new();
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await DeleteAsync($"products/{id}");
        }
        public async Task<Product?> AddProductAsync(Product product)
        {
            return await PostAsync<Product, Product>("products", product);
        }
        public async Task<List<Product>> GetAllProductsAsync() => await GetAsync<List<Product>>("products") ?? new();
        public async Task<Product?> GetProductByIdAsync(int id) => await GetAsync<Product>($"products/{id}");
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId) => await GetAsync<List<Product>>($"products?categoryId={categoryId}") ?? new();

        public async Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId) => await GetAsync<List<ProductReview>>($"products/{productId}/reviews") ?? new();
        public async Task<ProductReview?> SubmitReviewAsync(ProductReview review) => await PostAsync<ProductReview, ProductReview>($"products/{review.ProductId}/reviews", review);

        public async Task<List<ShoppingList>> GetShoppingListsByUserIdAsync(int userId) => await GetAsync<List<ShoppingList>>($"shoppinglists?userId={userId}") ?? new();
        public async Task<ShoppingList?> GetShoppingListByIdAsync(int listId) => await GetAsync<ShoppingList>($"shoppinglists/{listId}");
        public async Task<bool> CreateShoppingListAsync(ShoppingList newList) => await PostReturnBoolAsync("shoppinglists", newList);
        public async Task<bool> UpdateShoppingListAsync(ShoppingList updatedList) => await PutAsync($"shoppinglists/{updatedList.Id}", updatedList);
        public async Task<bool> DeleteShoppingListAsync(int listId) => await DeleteAsync($"shoppinglists/{listId}");
        public async Task<bool> AddProductToShoppingListAsync(int listId, ShoppingListItem item) => await PostReturnBoolAsync($"shoppinglists/{listId}/items", item);
        public async Task<bool> UpdateShoppingListItemAsync(int listId, ShoppingListItem item) => await PutAsync($"shoppinglists/{listId}/items/{item.Id}", item);
        public async Task<bool> DeleteShoppingListItemAsync(int listId, int itemId) => await DeleteAsync($"shoppinglists/{listId}/items/{itemId}");

        public async Task<string> GetTextFromApiAsync(string textKey) => (await GetAsync<AppTextDto>($"texts/{textKey}"))?.Content ?? "Metin bulunamadı.";

        public async Task<List<TipCategory>> GetTipCategoriesAsync() => await GetAsync<List<TipCategory>>("tips/categories") ?? new();
        public async Task<Tip?> PostTipAsync(Tip newTip) => await PostAsync<Tip, Tip>("tips", newTip);
        public async Task<bool> LikeTipAsync(int tipId) => await PostAsync($"tips/{tipId}/like");
        public async Task<bool> DislikeTipAsync(int tipId) => await PostAsync($"tips/{tipId}/dislike");
        public async Task<List<Tip>> GetApprovedTipsAsync() => await GetAsync<List<Tip>>("tips") ?? new();
        public async Task<Tip?> GetTipByIdAsync(int tipId) => await GetAsync<Tip>($"tips/{tipId}");

        public async Task<List<NewsArticle>> GetNewsArticlesAsync(string? searchText = null)
        {
            var endpoint = "news";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                endpoint += $"?searchText={Uri.EscapeDataString(searchText)}";
            }
            return await GetAsync<List<NewsArticle>>(endpoint) ?? new List<NewsArticle>();
        }
        public async Task<NewsArticle?> GetNewsArticleByIdAsync(int articleId) => await GetAsync<NewsArticle>($"news/{articleId}");
        public async Task<List<NewsReview>> GetReviewsForArticleAsync(int articleId) => await GetAsync<List<NewsReview>>($"news/{articleId}/reviews") ?? new();
        public async Task<bool> LikeArticleAsync(int articleId) => await PostAsync($"news/{articleId}/like");
        public async Task<bool> DislikeArticleAsync(int articleId) => await PostAsync($"news/{articleId}/dislike");
        public async Task<NewsReview?> SubmitReviewAsync(int articleId, int rating, string comment)
        {
            var reviewData = new { Rating = rating, Comment = comment };
            return await PostAsync<object, NewsReview>($"news/{articleId}/reviews", reviewData);
        }
    }
}