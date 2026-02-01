using System.ComponentModel.DataAnnotations;

namespace MedLink.Application.DTOs.User;

public class FavoriteDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a positive number.")]
    public int DoctorId { get; set; }
}
