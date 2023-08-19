using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs
{
    public record AddStudentRequestDTO(
        [Required]
        string Name,
        [Required]
        [StringLength(20)]
        string Gender,
        [Range(0, 200)]
        int Age);
   
}
