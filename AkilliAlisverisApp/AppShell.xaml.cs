using AkilliAlisverisApp.Views;
using Microsoft.Maui.Controls;
using System;

namespace AkilliAlisverisApp
{
    public partial class AppShell : Shell
    {
        private readonly ToolbarItem _loginToolbarItem;
        private readonly ToolbarItem _profileToolbarItem;

        public AppShell()
        {
            InitializeComponent();

            // Tüm Rota Kayıtlarını merkezileştirdik
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(ProductCategoryPage), typeof(ProductCategoryPage));
            Routing.RegisterRoute(nameof(ShoppingListItemView), typeof(ShoppingListItemView));
            Routing.RegisterRoute(nameof(NewsDetailView), typeof(NewsDetailView));
            Routing.RegisterRoute(nameof(TipDetailView), typeof(TipDetailView));
            Routing.RegisterRoute(nameof(TipsInsertView), typeof(TipsInsertView));
            Routing.RegisterRoute(nameof(ComplaintInsertPage), typeof(ComplaintInsertPage));
            Routing.RegisterRoute(nameof(UserView), typeof(UserView)); // UserView için de rota eklemek en doğrusu

            // Toolbar item'larını oluşturan kodunuz (doğru ve gerekli)
            _loginToolbarItem = new ToolbarItem
            {
                Text = "Giriş",
                IconImageSource = "login_icon.svg",
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                // Navigasyonun her zaman çalışması için mutlak rota ("//") kullanmak daha güvenlidir.
                Command = new Command(async () => await Shell.Current.GoToAsync($"{nameof(LoginPage)}"))
            };

            _profileToolbarItem = new ToolbarItem
            {
                Text = "Profil",
                IconImageSource = "profile_icon.svg",
                Order = ToolbarItemOrder.Primary,
                Priority = 1,
                // Navigasyonun her zaman çalışması için mutlak rota ("//") kullanmak daha güvenlidir.
                Command = new Command(async () => await Shell.Current.GoToAsync($"{nameof(UserView)}"))
            };

            // Sayfa değişimlerini dinleyen event (doğru ve gerekli)
            this.Navigated += OnShellNavigated;
        }

        // Toolbar item'larını yöneten tüm metotlarınız (doğru ve gerekli)
        private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            UpdateToolbarItemVisibility();
        }

        public void UpdateLoginStatus(bool isLoggedIn)
        {
            Preferences.Set("IsLoggedIn", isLoggedIn);
            MainThread.BeginInvokeOnMainThread(UpdateToolbarItemVisibility);
        }

        private void UpdateToolbarItemVisibility()
        {
            if (Shell.Current == null || Shell.Current.ToolbarItems == null)
            {
                return;
            }

            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            string currentRoute = Shell.Current?.CurrentState?.Location?.OriginalString ?? string.Empty;

            bool onAuthPage = currentRoute.Contains(nameof(LoginPage)) || currentRoute.Contains(nameof(RegisterPage));

            // Önce tüm butonları temizle
            if (ToolbarItems.Contains(_loginToolbarItem)) ToolbarItems.Remove(_loginToolbarItem);
            if (ToolbarItems.Contains(_profileToolbarItem)) ToolbarItems.Remove(_profileToolbarItem);

            // Auth sayfalarında değilsen ilgili butonu ekle
            if (!onAuthPage)
            {
                if (isLoggedIn)
                {
                    ToolbarItems.Add(_profileToolbarItem);
                }
                else
                {
                    ToolbarItems.Add(_loginToolbarItem);
                }
            }
        }
    }
}