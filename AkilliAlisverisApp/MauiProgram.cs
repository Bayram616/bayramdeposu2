using AkilliAlisverisApp;
using AkilliAlisverisApp.Converters;
using AkilliAlisverisApp.Helpers;
using AkilliAlisverisApp.Services;
using AkilliAlisverisApp.ViewModels;
using AkilliAlisverisApp.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using AkilliAlisverisApp.Views.Reviews;
using AkilliAlisverisApp.ViewModels.Reviews;
using System.Net.Http;
using System.Threading.Tasks;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- SERVİS KAYITLARI BAŞLANGIÇ (DÜZELTİLMİŞ BÖLÜM) ---

        // 1. Hata ayıklama handler'ı
        builder.Services.AddTransient<DebuggingHandler>();

        // 2. HttpClient'ı, handler zinciriyle birlikte tek bir singleton olarak kaydediyoruz.
        builder.Services.AddSingleton(provider =>
        {
            var handler = provider.GetRequiredService<DebuggingHandler>();
            handler.InnerHandler = new HttpClientHandler();

            return new HttpClient(handler)
            {
                BaseAddress = new Uri("https://byrappi-ctcbbmhrfme8b5cr.germanywestcentral-01.azurewebsites.net/api/")
            };
        });

        // 3. ApiService'in kendisini tek örnek olarak kaydediyoruz.
        builder.Services.AddSingleton<ApiService>(provider =>
        {
            var httpClient = provider.GetRequiredService<HttpClient>();
            var apiService = new ApiService(httpClient);

            // Token yönetimi App.xaml.cs'deki OnStart metoduna taşındığı için burada doğrudan yapılmıyor.
            // Bu kısım artık ana iş parçacığını bloklamıyor.

            return apiService;
        });

        // 4. Tüm arayüzlerin, yukarıda oluşturulan TEK ApiService örneğini kullanmasını garanti altına alıyoruz.
        builder.Services.AddSingleton<ILocationService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IUserService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IMarketService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IProductService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<ITipService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IShoppingListService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IProductReviewService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<INewsService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<ITextService>(sp => sp.GetRequiredService<ApiService>());
        builder.Services.AddSingleton<IApiService, ApiService>();
        // HttpClient zaten yukarıda singleton olarak ekleniyor, bu satır gereksiz
        // builder.Services.AddSingleton<HttpClient>();

        // ApiService'e bağlı olmayan diğer servisler
        builder.Services.AddSingleton<IComplaintService, ComplaintService>();
        builder.Services.AddSingleton<ISasService, SasService>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<IAppPopupService, PopupService>();
        builder.Services.AddSingleton<IMyNotificationService, NotificationService>();

        builder.Services.AddSingleton<WeakReferenceMessenger>();

        // Converters
        builder.Services.AddSingleton<BoolToGlobalMarketTextConverter>();
        builder.Services.AddSingleton<IsNotNullOrEmptyConverter>();
        builder.Services.AddSingleton<IsGreaterThanZeroConverter>();
        builder.Services.AddSingleton<IsNotNullConverter>();
        builder.Services.AddSingleton<NullToBoolConverter>();
        builder.Services.AddSingleton<StringToBoolConverter>();

        // ViewModels
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ProductViewModel>();
        builder.Services.AddTransient<CityViewModel>();
        builder.Services.AddTransient<ProductCategoryViewModel>();
        builder.Services.AddTransient<ProductDetailViewModel>();
        builder.Services.AddTransient<TipsViewModel>();
        builder.Services.AddTransient<TipCategoryViewModel>();
        builder.Services.AddTransient<TipsInsertViewModel>();
        builder.Services.AddTransient<ShoppingListItemViewModel>();
        builder.Services.AddTransient<ShoppingListViewModel>();
        builder.Services.AddTransient<UserViewModel>();
        builder.Services.AddTransient<ComplaintViewModel>();
        builder.Services.AddTransient<ComplaintInsertViewModel>();
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<ProductReviewViewModel>();
        builder.Services.AddTransient<NewsDetailView>();

        // Views
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ProductView>();
        builder.Services.AddTransient<ProductCategoryPage>();
        builder.Services.AddTransient<ProductDetailPage>();
        builder.Services.AddTransient<TipsView>();
        builder.Services.AddTransient<TipDetailView>();
        builder.Services.AddTransient<TipsInsertView>();
        builder.Services.AddTransient<CityView>();
        builder.Services.AddTransient<ShoppingListItemView>();
        builder.Services.AddTransient<ShoppingListView>();
        builder.Services.AddTransient<UserView>();
        builder.Services.AddTransient<ComplaintPage>();
        builder.Services.AddTransient<ComplaintInsertPage>();
        builder.Services.AddTransient<LegalWarningPopup>();
        builder.Services.AddTransient<NewsView>();
        builder.Services.AddTransient<ProductReviewsPage>();
        builder.Services.AddTransient<NewsDetailView>();

        // --- SERVİS KAYITLARI BİTİŞ ---

        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}