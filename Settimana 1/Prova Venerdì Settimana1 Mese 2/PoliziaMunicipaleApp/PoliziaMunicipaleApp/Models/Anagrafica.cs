using System.ComponentModel.DataAnnotations;

namespace PoliziaMunicipaleApp.Models
{
    public class Anagrafica
    {
        [Key]
        public int Idanagrafica { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri.")]
        public string? Cognome { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri.")]
        public string? Nome { get; set; }

        [StringLength(100, ErrorMessage = "L'indirizzo non può superare i 100 caratteri.")]
        public string? Indirizzo { get; set; }

        [StringLength(50, ErrorMessage = "La città non può superare i 50 caratteri.")]
        public string? Citta { get; set; }

        [StringLength(10, ErrorMessage = "Il CAP non può superare i 10 caratteri.")]
        public string? CAP { get; set; }

        [StringLength(16, ErrorMessage = "Il codice fiscale non può superare i 16 caratteri.")]
        public string? CodiceFiscale { get; set; }
    }
}
