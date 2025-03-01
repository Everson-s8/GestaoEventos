using GestaoEventos.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestaoEventos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<EventStaff> EventStaffs { get; set; }
        public DbSet<EventProduct> EventProducts { get; set; }
        public DbSet<EventNotification> EventNotifications { get; set; }
        public object EventProduct { get; internal set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Creator)
                .WithMany()  
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
            .HasOne(o => o.Buyer)
            .WithMany() // ou, se a classe User tiver uma coleção de Orders: .WithMany(u => u.Orders)
            .HasForeignKey(o => o.BuyerId)
            .IsRequired(false)   // torna opcional
            .OnDelete(DeleteBehavior.SetNull); // se o usuário for excluído, o BuyerId será nulo

            // Configuração da relação OrderItem -> EventProduct
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany() // ou .WithMany(p => p.OrderItems) se EventProduct tiver uma coleção de OrderItems
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }




    }
}
