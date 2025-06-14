using System.ComponentModel.DataAnnotations;

namespace ERegistration.Data
{
    public class BasicInfo
    {
        [Required(ErrorMessage = "الرجاء إختيار الجنس")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار الدولة")]
        public string CountryId { get; set; }
        public string StateId { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار الديانة")]
        public string ReligionId { get; set; }
        [Required(ErrorMessage ="الرجاء إدخال العنوان")]
        public string Address { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage ="الرجاء إختيار تاريخ الميلاد")]
        public DateTime Birthdate { get; set; }
    }
}
