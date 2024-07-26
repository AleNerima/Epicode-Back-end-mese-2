using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Il codice fiscale è obbligatorio")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri")]
        public string? CodiceFiscale { get; set; }

        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
        public string? Cognome { get; set; }

        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
        public string? Nome { get; set; }

        [StringLength(50, ErrorMessage = "La città non può superare i 50 caratteri")]
        public string? Citta { get; set; }

        [StringLength(50, ErrorMessage = "La provincia non può superare i 50 caratteri")]
        public string? Provincia { get; set; }

        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [StringLength(100, ErrorMessage = "L'email non può superare i 100 caratteri")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Numero di telefono non valido")]
        [StringLength(20, ErrorMessage = "Il telefono non può superare i 20 caratteri")]
        public string? Telefono { get; set; }

        [Phone(ErrorMessage = "Numero di cellulare non valido")]
        [StringLength(20, ErrorMessage = "Il cellulare non può superare i 20 caratteri")]
        public string? Cellulare { get; set; }
    }
}
