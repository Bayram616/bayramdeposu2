using CommunityToolkit.Mvvm.ComponentModel;

namespace AkilliAlisverisApp.Models
{
    public partial class NewsArticle : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }

        [ObservableProperty]
        private int _likeCount;

        [ObservableProperty]
        private int _dislikeCount;

        [ObservableProperty]
        private double _averageRating;

        [ObservableProperty]
        private int _commentCount;

        // XAML'de kısa içeriği göstermek için yardımcı bir özellik
        public string ShortContent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content))
                    return string.Empty;

                var words = Content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return string.Join(" ", words.Take(10)) + (words.Length > 10 ? "..." : "");
            }
        }
    }
}