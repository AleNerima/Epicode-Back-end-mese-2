using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class Prenotazione
    {
        [Key]
        public int IdPrenotazione { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [Required]
        public int IdCamera { get; set; }

        [ForeignKey("IdCamera")]
        public Camera? Camera { get; set; }

        [Required(ErrorMessage = "La data della prenotazione è obbligatoria")]
        public DateTime DataPrenotazione { get; set; }

        [Required(ErrorMessage = "Il numero progressivo è obbligatorio")]
        public int NumeroProgressivo { get; set; }

        [Required(ErrorMessage = "L'anno è obbligatorio")]
        public int Anno { get; set; }

        [Required(ErrorMessage = "La data di inizio soggiorno è obbligatoria")]
        public DateTime PeriodoSoggiornoDal { get; set; }

        [Required(ErrorMessage = "La data di fine soggiorno è obbligatoria")]
        public DateTime PeriodoSoggiornoAl { get; set; }

        [Required(ErrorMessage = "La caparra confirmatoria è obbligatoria")]
        [Range(0, 999999.99, ErrorMessage = "La caparra confirmatoria deve essere un valore positivo")]
        public decimal CaparraConfirmatoria { get; set; }

        [Required(ErrorMessage = "La tariffa è obbligatoria")]
        [Range(0, 999999.99, ErrorMessage = "La tariffa deve essere un valore positivo")]
        public decimal Tariffa { get; set; }

        [Required(ErrorMessage = "Il tipo di soggiorno è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il tipo di soggiorno non può superare i 50 caratteri")]
        public string? TipoSoggiorno { get; set; }

        [Required(ErrorMessage = "Lo stato della prenotazione è obbligatorio")]
        [StringLength(20, ErrorMessage = "Lo stato della prenotazione non può superare i 20 caratteri")]
        public string? Stato { get; set; }
        
    }
}
