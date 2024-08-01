using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class Prodotto
    {
        [Key]
        public int ProdottoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } // Non è più nullable

        public string FotoBase64 { get; set; } // Memorizza l'immagine in formato Base64 (non nullable)

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Prezzo { get; set; }

        [Required]
        public int TempoConsegna { get; set; } // Tempo di consegna in minuti

        public string Ingredienti { get; set; } // Ingredienti principali del prodotto (non nullable)

        // Relazione con DettaglioOrdine
        public ICollection<DettaglioOrdine> DettagliOrdine { get; set; } = new List<DettaglioOrdine>(); // Inizializzazione per evitare null reference
    }
}
