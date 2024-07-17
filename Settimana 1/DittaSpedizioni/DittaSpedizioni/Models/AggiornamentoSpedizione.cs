namespace DittaSpedizioni.Models
{
   
        public class AggiornamentoSpedizione
        {
            public int IdAggiornamento { get; set; }
            public int Spedizione { get; set; }
            public string? Stato { get; set; }
            public string? Luogo { get; set; }
            public string? Descrizione { get; set; }
            public DateTime DataAggiornamento { get; set; }
            public int Operatore { get; set; } // Campo aggiunto
        }
    


}
