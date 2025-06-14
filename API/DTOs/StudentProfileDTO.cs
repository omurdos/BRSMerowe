using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class StudentProfileDTO
    {
        //Personal Info

        [Required()]
        public string Gender { get; set; }
        [Required()]
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string ReligionId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        [Required()]
        public DateTime Birthdate { get; set; }
        //Guardian
        public Guardian Guardian { get; set; }
        //Attachments
        public StudentAttachment Attachments { get; set; }
        public string Accomodation { get; set; }
    }
}
