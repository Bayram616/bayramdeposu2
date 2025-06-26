using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Models
{
    public class ProductReview
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Product? Product { get; set; }

        public User? User { get; set; }
    }

}
