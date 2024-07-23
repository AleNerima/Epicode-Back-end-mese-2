using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        public string? Cognome { get; set; }
    }
}
