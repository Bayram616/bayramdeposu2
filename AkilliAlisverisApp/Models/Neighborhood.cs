using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class Neighborhood
    {
        [Key]
        public int NeighborhoodId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NeighborhoodName { get; set; } = string.Empty;

        [Required]
        public int DistrictId { get; set; }

        public virtual District District { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        // Picker'da bu nesnenin metinsel temsilini sağlamak için ToString() metodunu override ediyoruz.
        public override string ToString()
        {
            return NeighborhoodName;
        }
    }
}