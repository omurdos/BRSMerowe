using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateDeviceDTO
    {
        public string UserId { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string FCMToken { get; set; }
        [Required]
        public string OSVersion { get; set; }

    }
}
