using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }  // Chiave esterna per Order

        [Required]
        public int ProductId { get; set; }  // Chiave esterna per Product

        [Required]
        [Range(1, 100, ErrorMessage = "La quantità deve essere compresa tra 1 e 100.")]
        public int Quantità { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
