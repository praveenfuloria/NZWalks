using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code has to be a minimum 3 character")]
        [MinLength(3, ErrorMessage = "Code has to be a minimum 3 character")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum 100 character")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
