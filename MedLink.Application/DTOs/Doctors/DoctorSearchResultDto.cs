namespace MedLink.Application.DTOs.Doctors
{
    public class DoctorSearchResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool AvailableToday { get; set; }
        public string? ImageUrl { get; set; }
    }
}
