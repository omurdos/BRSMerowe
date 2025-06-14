using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ProfilePictureDTO
    {
        [Required]
        public string UserId { get; set; }
        public string StudentNumber { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
