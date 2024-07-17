namespace DittaSpedizioni.Models
{
    public class RegistrazioneUtente
    {
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfermaPassword { get; set; }
        public string? Ruolo { get; set; }
    }
}
