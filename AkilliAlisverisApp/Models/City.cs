using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CityName { get; set; } = string.Empty;

        public virtual ICollection<District> Districts { get; set; } = new List<District>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        // Picker'da bu nesnenin metinsel temsilini sağlamak için ToString() metodunu override ediyoruz.
        // Bu sayede "AkilliAlisverisApp.Models.City" yerine CityName görünecek.
        public override string ToString()
        {
            return CityName;
        }
    }
}