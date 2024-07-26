using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class Dipendente
    {
        [Key]
        public int IdDipendente { get; set; }

        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome utente non può superare i 50 caratteri")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [StringLength(255, ErrorMessage = "La password non può superare i 255 caratteri")]
        public string? PasswordHash { get; set; }

        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
        public string? Nome { get; set; }

        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
        public string? Cognome { get; set; }

        [StringLength(50, ErrorMessage = "Il ruolo non può superare i 50 caratteri")]
        public string? Ruolo { get; set; } 
    }
}
