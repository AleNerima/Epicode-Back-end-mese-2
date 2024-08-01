using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace PizzeriaApp.Models
{
    public class Utente
    {
        [Key]
        public int UtenteId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; } // Cambiato da Password a PasswordHash

        public bool IsAdmin { get; set; } // Utilizzato per distinguere gli amministratori

        // Relazione con Ordini
        public ICollection<Ordine> Ordini { get; set; }
    }

}