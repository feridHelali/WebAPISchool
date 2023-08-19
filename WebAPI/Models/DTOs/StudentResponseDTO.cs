using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs
{
    public record StudentResponseDTO(
        Guid Id,
        string Name,
        string Gender,
        int Age
        );
}
