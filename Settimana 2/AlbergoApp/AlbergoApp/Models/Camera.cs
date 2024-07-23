using System.ComponentModel.DataAnnotations;

namespace AlbergoApp.Models
{
    public class Camera
    {
        [Key]
        public int IdCamera { get; set; }

        [Required(ErrorMessage = "Il numero della camera è obbligatorio")]
        public int Numero { get; set; }

        public string? Descrizione { get; set; }

        [Required(ErrorMessage = "La tipologia è obbligatoria")]
        [StringLength(20, ErrorMessage = "La tipologia non può superare i 20 caratteri")]
        public string? Tipologia { get; set; }
    }
}
