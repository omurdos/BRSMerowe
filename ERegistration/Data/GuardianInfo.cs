using System.ComponentModel.DataAnnotations;

namespace ERegistration.Data
{
    public class GuardianInfo
    {
        [Required(ErrorMessage = "الرجاء إدخال الاسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال رقم الهاتف")]
        [MaxLength(9, ErrorMessage = "الرجاء إدخال 9 ارقام فقط")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال العنوان")]
        public string Address { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال صلة القرابة")]
        public string Relashionship { get; set; }
    }
}
