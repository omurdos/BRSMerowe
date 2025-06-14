using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ResetPasswordDTO
    {
        [Required()]
        public string PhoneNumber { get; set; }
        [Required()]
        public string Password { get; set; }
        [Required()]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
