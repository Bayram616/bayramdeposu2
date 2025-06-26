using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
