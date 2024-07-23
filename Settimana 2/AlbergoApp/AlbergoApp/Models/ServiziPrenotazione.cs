using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class ServiziPrenotazione
    {
        [Key]
        public int IdServiziPrenotazione { get; set; }

        [Required]
        public int IdPrenotazione { get; set; }

        [ForeignKey("IdPrenotazione")]
        public Prenotazione? Prenotazione { get; set; }

        [Required]
        public int IdServizio { get; set; }

        [ForeignKey("IdServizio")]
        public Servizio? Servizio { get; set; }

        [Required(ErrorMessage = "La data del servizio è obbligatoria")]
        public DateTime DataServizio { get; set; }

        [Required(ErrorMessage = "La quantità è obbligatoria")]
        [Range(1, 999, ErrorMessage = "La quantità deve essere almeno 1")]
        public int Quantita { get; set; }

        [Required(ErrorMessage = "Il prezzo unitario è obbligatorio")]
        [Range(0, 999999.99, ErrorMessage = "Il prezzo unitario deve essere un valore positivo")]
        public decimal PrezzoUnitario { get; set; }

        [Required(ErrorMessage = "Il prezzo totale è obbligatorio")]
        [Range(0, 999999.99, ErrorMessage = "Il prezzo totale deve essere un valore positivo")]
        public decimal PrezzoTotale { get; set; }
    }
}
