namespace Dashboard.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            UserName= string.Empty;
            Password= string.Empty;
        }
    }
}
