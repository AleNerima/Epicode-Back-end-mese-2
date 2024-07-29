using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Models;

namespace PizzeriaApp.Data
{   
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }
            public DbSet<User> Users { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configurazione per la tabella Product
                modelBuilder.Entity<Product>()
                    .HasKey(p => p.Id);

                modelBuilder.Entity<Product>()
                    .Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                modelBuilder.Entity<Product>()
                    .Property(p => p.Prezzo)
                    .HasPrecision(18, 2);

                // Configurazione per la tabella Order
                modelBuilder.Entity<Order>()
                    .HasKey(o => o.Id);

                modelBuilder.Entity<Order>()
                    .Property(o => o.OrderDate)
                    .IsRequired();

                modelBuilder.Entity<Order>()
                    .Property(o => o.IndirizzoSpedizione)
                    .IsRequired();

                modelBuilder.Entity<Order>()
                    .Property(o => o.Note)
                    .IsRequired()
                    .HasMaxLength(1000);

                // Configurazione per la tabella OrderItem
                modelBuilder.Entity<OrderItem>()
                    .HasKey(oi => oi.Id);

                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId);

                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId);

                // Configurazione per la tabella User
                modelBuilder.Entity<User>()
                    .HasKey(u => u.Id);

                modelBuilder.Entity<User>()
                    .Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                modelBuilder.Entity<User>()
                    .Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                modelBuilder.Entity<User>()
                    .Property(u => u.Telefono)
                    .IsRequired();

                modelBuilder.Entity<User>()
                    .Property(u => u.PasswordHash)
                    .IsRequired();

                modelBuilder.Entity<User>()
                    .Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(10);  // Lunghezza sufficiente per 'User' e 'Admin'

                // Configurazione della relazione tra User e Order
                modelBuilder.Entity<User>()
                    .HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId);
            }
        }
    }

