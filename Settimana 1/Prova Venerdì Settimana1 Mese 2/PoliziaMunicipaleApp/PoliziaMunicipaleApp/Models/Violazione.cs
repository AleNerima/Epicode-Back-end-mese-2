namespace PoliziaMunicipaleApp.Models
{
    public class Violazione
    {
        public int Idverbale { get; set; }
        public decimal Importo { get; set; }
        public string? Cognome { get; set; }
        public string? Nome { get; set; }
        public DateTime DataViolazione { get; set; }
        public int DecurtamentoPunti { get; set; }
    }
}
