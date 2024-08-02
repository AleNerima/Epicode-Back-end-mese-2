using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class User 
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri.")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "L'email non può superare i 256 caratteri.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Il ruolo deve essere uno tra 'User' e 'Admin'.")]
        public string Role { get; set; }  // 'User' o 'Admin'

        // Relazione con gli ordini
        public virtual ICollection<Order> Orders { get; set; }
    }
}
