using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliAlisverisApp.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        public int ListID { get; set; } // FK: ShoppingList.Id
        public int ProductID { get; set; } // FK: Product.Id (opsiyonel)

        public string? CustomProductName { get; set; } // Elle girilen ürün ismi
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public bool IsPurchased { get; set; }

        // 👇 İLİŞKİLER
        public ShoppingList ShoppingList { get; set; } // Navigation
        public Product? Product { get; set; } // Navigation (nullable çünkü özel ürün olabilir)
    }
}
