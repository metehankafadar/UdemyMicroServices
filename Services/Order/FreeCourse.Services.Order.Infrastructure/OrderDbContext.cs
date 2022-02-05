using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext:DbContext
    {

        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions options) : base(options)
        {

        }
        

        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);
            //16 karakter ve virgülden sonra 2 karakter toplam 18 decimal karakter olacak.
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

            //Order'ın Addresine yani domain içerisinde Address.cs 'ın Owner olduğunu tanımladık. Domainin hangi framework ile çalıştığını bilmesini istemediğimizden burada tanımlıyoruz.
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner();
            // işlemlerimizin hepsini order sınıfımın üzerinden yapacağız.

            base.OnModelCreating(modelBuilder);
        }

    }
}
