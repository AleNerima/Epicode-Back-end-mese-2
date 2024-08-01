using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Models;

namespace PizzeriaApp.Data
{
    public class PizzeriaContextDb : DbContext
    {
        public PizzeriaContextDb(DbContextOptions<PizzeriaContextDb> options)
            : base(options)
        {
        }

        public DbSet<Utente> Utenti { get; set; }  // Aggiungi il DbSet per Utenti
        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<DettaglioOrdine> DettagliOrdine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazioni per Utente
            modelBuilder.Entity<Utente>()
                .Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Utente>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256); // Default max length for emails

            modelBuilder.Entity<Utente>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100); // Assicurati che sia sufficientemente lungo per memorizzare le password hashate

            modelBuilder.Entity<Utente>()
                .Property(u => u.IsAdmin)
                .IsRequired();

            // Configurazioni per Prodotto
            modelBuilder.Entity<Prodotto>()
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Prodotto>()
                .Property(p => p.FotoBase64)
                .HasMaxLength(10000); // Lunghezza massima per il Base64

            modelBuilder.Entity<Prodotto>()
                .Property(p => p.Prezzo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Prodotto>()
                .Property(p => p.TempoConsegna)
                .IsRequired();

            // Configurazioni per Ordine
            modelBuilder.Entity<Ordine>()
                .Property(o => o.IndirizzoSpedizione)
                .IsRequired();

            modelBuilder.Entity<Ordine>()
                .Property(o => o.Note)
                .HasMaxLength(1000);

            // Configurazione della relazione tra Ordine e Utente
            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.Utente)
                .WithMany(u => u.Ordini)
                .HasForeignKey(o => o.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurazione delle relazioni tra DettaglioOrdine e Ordine, Prodotto
            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Ordine)
                .WithMany(o => o.DettagliOrdine)
                .HasForeignKey(d => d.OrdineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Prodotto)
                .WithMany(p => p.DettagliOrdine)
                .HasForeignKey(d => d.ProdottoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
