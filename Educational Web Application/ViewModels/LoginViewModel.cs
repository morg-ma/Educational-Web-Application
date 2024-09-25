using System.ComponentModel.DataAnnotations;

namespace EducationalWebApplication.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Your Username")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Enter Your Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
