namespace DittaSpedizioni.Models
{
    public class Utente
    {
        public int IdUtente { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Ruolo { get; set; }
    }
}
