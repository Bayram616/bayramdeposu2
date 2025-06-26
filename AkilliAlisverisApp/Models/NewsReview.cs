namespace AkilliAlisverisApp.Models
{
    public class NewsReview
    {
        public int Id { get; set; }
        public int NewsArticleId { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } // Yorumu yapanın adını göstermek için
        public int Rating { get; set; } // 1-5 arası puan
        public string Comment { get; set; }
        public DateTime PostedDate { get; set; }
    }
}