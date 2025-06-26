using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class ProductRating
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int UserId { get; set; }

        [Range(1, 5)]
        public int RatingValue { get; set; }

        public Product? Product { get; set; }
    }
}
