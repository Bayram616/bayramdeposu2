namespace AkilliAlisverisApp.Models.DTOs
{
    // Bu DTO'lar API'den kullanıcı detayı alınırken kullanılır.
    public class CityDto
    {
        public string CityName { get; set; }
    }

    public class DistrictDto
    {
        public string DistrictName { get; set; }
    }

    public class NeighborhoodDto
    {
        public string NeighborhoodName { get; set; }
    }

    public class UserDetailDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public CityDto City { get; set; }
        public DistrictDto District { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
    }
}