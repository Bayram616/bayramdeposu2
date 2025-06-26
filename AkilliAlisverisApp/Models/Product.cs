using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliAlisverisApp.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int MarketID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string MarketName { get; set; } = string.Empty;
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;
        public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new HashSet<ShoppingListItem>();
        public virtual ICollection<UserList> UserLists { get; set; } = new HashSet<UserList>();
    }
}