using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Il nome del prodotto non può superare i 100 caratteri.")]
        public string Nome { get; set; }

        
        public string Foto { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Il prezzo deve essere compreso tra 0.01 e 10000.")]
        public decimal Prezzo { get; set; }

        [Range(1, 120, ErrorMessage = "Il tempo di consegna deve essere compreso tra 1 e 120 minuti.")]
        public int TempoConsegna { get; set; }

        [StringLength(500, ErrorMessage = "Gli ingredienti non possono superare i 500 caratteri.")]
        public string Ingredienti { get; set; }
    }
}
