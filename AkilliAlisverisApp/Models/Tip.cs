using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AkilliAlisverisApp.Models
{
    public partial class Tip : ObservableObject
    {
        public int TipID { get; set; }
        public int? CategoryID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserID { get; set; }
        public bool? IsActive { get; set; }

        [ObservableProperty]
        private string _shortContent = string.Empty;

        [ObservableProperty]
        private int _likeCount;

        [ObservableProperty]
        private int _dislikeCount;

        [ObservableProperty]
        private int _commentCount;
    }
}