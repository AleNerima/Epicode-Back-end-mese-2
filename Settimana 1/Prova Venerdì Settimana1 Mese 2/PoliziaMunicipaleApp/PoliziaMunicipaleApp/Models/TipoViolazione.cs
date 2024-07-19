using System.ComponentModel.DataAnnotations;

namespace PoliziaMunicipaleApp.Models
{
    public class TipoViolazione
    {
        [Key]
        public int Idviolazione { get; set; }

        [StringLength(100, ErrorMessage = "La descrizione non può superare i 100 caratteri.")]
        public string? Descrizione { get; set; }
    }
}
