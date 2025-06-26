using System;
using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class ProductComment
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int UserId { get; set; }

        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Product? Product { get; set; }
    }
}
