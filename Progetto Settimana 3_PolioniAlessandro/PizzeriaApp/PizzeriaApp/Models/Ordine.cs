using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaApp.Models
{
    public class Ordine
    {
        [Key]
        public int OrdineId { get; set; }

        [Required]
        public int UtenteId { get; set; } // Cambiato in string per allinearsi con IdentityUser

        public string? IndirizzoSpedizione { get; set; } // Ora nullable

        public string? Note { get; set; } // Ora nullable

        public bool Evaso { get; set; } // Stato dell'ordine: se è stato evaso o meno

        // Relazione con Utente
        public Utente Utente { get; set; } // Non è più nullable

        // Relazione con DettaglioOrdine
        public ICollection<DettaglioOrdine> DettagliOrdine { get; set; } = new List<DettaglioOrdine>(); // Inizializzazione per evitare null reference
    }
}
