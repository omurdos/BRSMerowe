using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SignupDTO
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StudentNumberOrFormAddmission { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        public CreateDeviceDTO Device { get; set; }
    }
}
