using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliAlisverisApp.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }

        public int UserID { get; set; } // Kullanıcı ilişkisi için FK
        public int MarketID { get; set; }
        public string ListName { get; set; } = string.Empty;
        public string MarketName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }

        // 👇 İLİŞKİLER
        public User User { get; set; } // Navigation property (User → ShoppingLists)
        public Market Market { get; set; } // Navigation property (Market → ShoppingLists)

        public ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
    }
}
