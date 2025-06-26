using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliAlisverisApp.Models
{
    public class Market
    {
        [Key]
        public int MarketID { get; set; }

        [Required]
        [MaxLength(100)]
        public string MarketName { get; set; } = string.Empty;

        public bool IsGlobalMarket { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        public virtual ICollection<MarketNeighborhood> MarketNeighborhoods { get; set; } = new List<MarketNeighborhood>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}