using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducationalWebApplication.ViewModels
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "This Doesn't Match the Password Field!")]
        public string ConfirmPassword { get; set; }
    }
}
