using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string? PhoneNumber { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        public string? Gender { get; set; } = string.Empty;

        public int? CityId { get; set; }

        public int? DistrictId { get; set; }

        public int NeighborhoodId { get; set; }

        public virtual City? City { get; set; }

        public virtual District? District { get; set; }

        public virtual Neighborhood? Neighborhood { get; set; }

        public string? Token { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new HashSet<ShoppingList>();
        public virtual ICollection<UserList> UserLists { get; set; } = new HashSet<UserList>();
    }
}