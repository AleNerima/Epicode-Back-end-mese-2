using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
