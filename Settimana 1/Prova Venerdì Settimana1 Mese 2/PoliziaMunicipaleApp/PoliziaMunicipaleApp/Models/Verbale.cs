using System.ComponentModel.DataAnnotations;

namespace PoliziaMunicipaleApp.Models
{
    public class Verbale
    {
        [Key]
        public int Idverbale { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataViolazione { get; set; }

        [StringLength(100, ErrorMessage = "L'indirizzo della violazione non può superare i 100 caratteri.")]
        public string? IndirizzoViolazione { get; set; }

        [StringLength(50, ErrorMessage = "Il nominativo dell'agente non può superare i 50 caratteri.")]
        public string? NominativoAgente { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataTrascrizioneVerbale { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "L'importo deve essere un valore positivo.")]
        public decimal? Importo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Il decurtamento punti deve essere un valore positivo.")]
        public int? DecurtamentoPunti { get; set; }

        public int? Idanagrafica { get; set; }
        public int? Idviolazione { get; set; }

        public Anagrafica? Anagrafica { get; set; }
        public TipoViolazione? TipoViolazione { get; set; }
    }
}
