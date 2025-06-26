namespace AkilliAlisverisApp.Models
{
    public class MarketLogoDto
    {
        public int MarketId { get; set; }
        public string? MarketName { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class MarketSelectionDto
    {
        public int MarketId { get; set; }
        public string? MarketName { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsGlobalMarket { get; set; }
    }
}