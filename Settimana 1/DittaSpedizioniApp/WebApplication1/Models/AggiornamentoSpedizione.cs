using System.ComponentModel.DataAnnotations;

namespace DittaSpedizioniApp.Models
{
    public class AggiornamentoSpedizione
    {
        public int IdAggiornamento { get; set; }

        [Required(ErrorMessage = "La spedizione è obbligatoria")]
        public int Spedizione { get; set; }

        [Required(ErrorMessage = "Lo stato è obbligatorio")]
        [StringLength(50, ErrorMessage = "Lo stato non può superare i 50 caratteri")]
        public string? Stato { get; set; }

        [Required(ErrorMessage = "Il luogo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il luogo non può superare i 100 caratteri")]
        public string? Luogo { get; set; }

        [StringLength(500, ErrorMessage = "La descrizione non può superare i 500 caratteri")]
        public string? Descrizione { get; set; }

        [Required]
        public DateTime DataAggiornamento { get; set; }
    }
}
