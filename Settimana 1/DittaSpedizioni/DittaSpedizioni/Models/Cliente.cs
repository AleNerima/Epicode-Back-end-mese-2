namespace DittaSpedizioni.Models
{
    
        public class Cliente
        {
            public int IdCliente { get; set; }
            public string? Nome { get; set; }
            public string? Tipo { get; set; } // Privato o Azienda
            public string? CodiceFiscale { get; set; } // Null se Azienda
            public string? PartitaIVA { get; set; } // Null se Privato
        }
    

}
