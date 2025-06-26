namespace AkilliAlisverisApp.Models
{
    public class Complaint
    {
        public string? Id { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public string? UserName { get; set; }
    }
}