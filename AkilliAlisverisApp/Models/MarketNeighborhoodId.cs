using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliAlisverisApp.Models
{
    public class MarketNeighborhood
    {
        [Key]
        public int MarketNeighborhoodId { get; set; }

        [Required]
        public int MarketId { get; set; }
        [ForeignKey("MarketId")]
        public virtual Market Market { get; set; } = null!;

        [Required]
        public int NeighborhoodId { get; set; }
        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood Neighborhood { get; set; } = null!;
    }
}