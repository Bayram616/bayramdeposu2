using AkilliAlisverisApp.Models;
using System.Collections.Generic;

namespace AkilliAlisverisApp
{
    public static class StaticData
    {
        // RegisterViewModel için Cinsiyet listesi
        public static List<string> Genders { get; } = new List<string>
        {
            "Erkek",
            "Kadın",
            "Diğer"
        };

        // ComplaintViewModel için Şikayet Kategorileri listesi
        public static List<ComplaintCategory> ComplaintCategories { get; } = new List<ComplaintCategory>
        {
            new ComplaintCategory { Name = "Ürün Şikayetleri", Icon = "product_icon.svg" },
            new ComplaintCategory { Name = "Marka Şikayetleri", Icon = "brand_icon.svg" },
            new ComplaintCategory { Name = "Satıcı Şikayetleri", Icon = "seller_icon.svg" },
            new ComplaintCategory { Name = "Diğer Şikayetler", Icon = "other_icon.svg" }
        };

        // ProductViewModel için Ürün Kategorileri listesi
        public static List<string> ProductCategories { get; } = new List<string>
        {
            "Aktüel Ürünler",
            "Temel Gıda",
            "İçecek",
            "Şarküteri",
            "Atıştırmalık ve Tatlı",
            "Temizlik Ürünleri",
            "Kişisel Bakım Ürünleri",
            "Diğer Ürünler"
        };
    }
}