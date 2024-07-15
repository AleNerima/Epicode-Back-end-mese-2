using System.ComponentModel.DataAnnotations;

namespace DittaSpedizioniApp.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Il tipo è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il tipo non può superare i 50 caratteri")]
        public string? Tipo { get; set; }

        [StringLength(16, ErrorMessage = "Il codice fiscale non può superare i 16 caratteri")]
        public string? CodiceFiscale { get; set; }

        [StringLength(11, ErrorMessage = "La partita IVA non può superare gli 11 caratteri")]
        public string? PartitaIVA { get; set; }
    }
}
