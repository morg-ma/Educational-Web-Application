using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducationalWebApplication.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Your Username")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }

        public string? Error {  get; set; }
    }
}
