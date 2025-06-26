using System;
using System.ComponentModel.DataAnnotations;

namespace AkilliAlisverisApp.Models
{
    public class UserList
    {
        [Key]
        public int ListID { get; set; }
        public int? UserID { get; set; }
        public int? ProductID { get; set; }
        public string MarketName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime? AddedAt { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}