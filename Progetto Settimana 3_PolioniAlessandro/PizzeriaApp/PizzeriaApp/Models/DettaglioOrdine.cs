using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class DettaglioOrdine
    {
        [Key]
        public int DettaglioOrdineId { get; set; }

        [Required]
        public int OrdineId { get; set; }

        [Required]
        public int ProdottoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantita { get; set; } // Quantità del prodotto

        // Relazione con Ordine
        public Ordine Ordine { get; set; } // Non è più nullable

        // Relazione con Prodotto
        public Prodotto Prodotto { get; set; } // Non è più nullable
    }
}
