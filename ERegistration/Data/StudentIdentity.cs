using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace ERegistration.Data
{
    public class StudentIdentity
    {
        public IBrowserFile PersonalPhoto { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال رقم الهاتف")]
        [MaxLength(9, ErrorMessage = "رقم الهاتف يجب ان يكون من 9 ارقام فقط")]
        public string PhoneNumber { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true",
        ErrorMessage = "الرجاء الاطلاع والموافقة على الشروط والاحكام")]
        public bool AcceptRegulations { get; set; }
    }
}
