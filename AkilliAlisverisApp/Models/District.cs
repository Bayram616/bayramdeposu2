using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DistrictName { get; set; } = string.Empty;

        public int CityId { get; set; }

        public virtual City? City { get; set; }
        public virtual ICollection<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        // Picker'da bu nesnenin metinsel temsilini sağlamak için ToString() metodunu override ediyoruz.
        public override string ToString()
        {
            return DistrictName;
        }
    }
}