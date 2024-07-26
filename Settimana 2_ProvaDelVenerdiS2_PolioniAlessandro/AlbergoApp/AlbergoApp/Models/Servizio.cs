using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class Servizio
    {
        [Key]
        public int IdServizio { get; set; }

        [Required(ErrorMessage = "Il nome del servizio è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome del servizio non può superare i 100 caratteri")]
        public string? NomeServizio { get; set; }

        [Required(ErrorMessage = "Il prezzo del servizio è obbligatorio")]
        [Range(0, 999999.99, ErrorMessage = "Il prezzo deve essere un valore positivo")]
        public decimal Prezzo { get; set; }
    }
}
