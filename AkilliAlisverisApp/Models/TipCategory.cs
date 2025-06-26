// AkilliAlisverisAPI.Models/TipCategory.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // [Table] niteliği için

namespace AkilliAlisverisApp.Models
{

    public class TipCategory // Sınıf adı TipCategory olarak kalacak
    {
        [Key]
        public int CategoryID { get; set; } // Anahtar

        [Required]
        [StringLength(100)] // Kategori adı için uygun bir uzunluk
        public string CategoryName { get; set; } // Kategori adı
    }
}