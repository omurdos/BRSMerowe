using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERegistration.Data
{
    public class AdmissionForm
    {
        [Required(ErrorMessage ="الرجاء إدخال رقم الاستمارة"), DataType(DataType.Text)]
        [DisplayName("رقم الإستمارة")]
        public string Number { get; set; }
    }
}
