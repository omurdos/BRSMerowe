using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> RoleNames { get; set; }

    }

    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password), Required]
        public string ConfirmPassword { get; set; }

    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }


}
