using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La {0} deve essere lunga almeno {2} caratteri e al massimo {1} caratteri.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma Password")]
        [Compare("Password", ErrorMessage = "La password e la conferma password non coincidono.")]
        public string ConfirmPassword { get; set; }
    }
}
