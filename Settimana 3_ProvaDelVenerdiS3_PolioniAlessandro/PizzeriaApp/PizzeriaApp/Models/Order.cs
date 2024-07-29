using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Chiave esterna per User

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Le note non possono superare i 1000 caratteri.")]
        public string Note { get; set; }

        [Required]
        public string IndirizzoSpedizione { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
