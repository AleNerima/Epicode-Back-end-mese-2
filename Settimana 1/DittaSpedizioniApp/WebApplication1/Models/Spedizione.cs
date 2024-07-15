using System.ComponentModel.DataAnnotations;

namespace DittaSpedizioniApp.Models
{
    public class Spedizione
    {
        public int IdSpedizione { get; set; }

        [Required(ErrorMessage = "Il cliente è obbligatorio")]
        public int Cliente { get; set; }

        [Required(ErrorMessage = "Il numero identificativo è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il numero identificativo non può superare i 50 caratteri")]
        public string? NumeroIdentificativo { get; set; }

        [Required(ErrorMessage = "La data di spedizione è obbligatoria")]
        public DateTime DataSpedizione { get; set; }

        [Required(ErrorMessage = "Il peso è obbligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il peso deve essere maggiore di zero")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "La città destinataria è obbligatoria")]
        [StringLength(100, ErrorMessage = "La città destinataria non può superare i 100 caratteri")]
        public string? CittaDestinataria { get; set; }

        [Required(ErrorMessage = "L'indirizzo destinatario è obbligatorio")]
        [StringLength(200, ErrorMessage = "L'indirizzo destinatario non può superare i 200 caratteri")]
        public string? IndirizzoDestinatario { get; set; }

        [Required(ErrorMessage = "Il nominativo destinatario è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nominativo destinatario non può superare i 100 caratteri")]
        public string? NominativoDestinatario { get; set; }

        [Required(ErrorMessage = "Il costo è obbligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il costo deve essere maggiore di zero")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "La data di consegna prevista è obbligatoria")]
        public DateTime DataConsegnaPrevista { get; set; }
    }
}
