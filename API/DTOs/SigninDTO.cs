namespace API.DTOs
{
    public class SigninDTO
    {
        public string PhoneNumber { get; set; }
        public string StudentNumber { get; set; }
        public string Password { get; set; }
        public CreateDeviceDTO Device { get; set; }
    }
}
