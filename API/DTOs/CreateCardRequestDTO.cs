using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateCardRequestDTO
    {
        public string Id { get; set; }
        [Required]
        public string StudentNumber { get; set; }
        [Required]
        public string Photo { get; set; }

    }

    public class UpdateCardRequestDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string StudentNumber { get; set; }
        public string Photo { get; set; }

    }

}
