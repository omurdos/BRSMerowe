using System.ComponentModel.DataAnnotations;

namespace ERegistration.Data
{
    public class FitnessForm
    {
        [Required(ErrorMessage = "الرجاء ادخال رقم التذكرة")]
        [MinLength(3, ErrorMessage = "رقم التذكرة يجب ان يتكون من 3 احرف على الاقل ")]
        public string TicketNo { get; set; }
    }
}
