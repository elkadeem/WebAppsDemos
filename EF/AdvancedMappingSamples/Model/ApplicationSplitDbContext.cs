using AdvancedMappingSamples.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMappingSamples.Model
{
    public class ApplicationSplitDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<DetailedOrder> DetailedOrders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        // This method is called by the runtime. Use this method to configure the HTTP request pipeline.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AdvancedMappingSamples;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .ToTable("Orders");

            modelBuilder.Entity<DetailedOrder>()
                .ToTable("Orders");

            modelBuilder.Entity<Order>()
                .HasOne(o => o.DetailedOrder)
                .WithOne()
                .HasForeignKey<DetailedOrder>(c => c.Id)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasColumnName("Status")
                .HasConversion<string>();

            modelBuilder.Entity<DetailedOrder>()
                .Property(o => o.Status)
                .HasColumnName("Status")
                .HasConversion<string>();

            modelBuilder.Entity<Customer>(
    entityBuilder =>
    {
        entityBuilder.Property(c => c.Street).HasMaxLength(50);
        entityBuilder.Property(c => c.City).HasMaxLength(50);
        entityBuilder.Property(c => c.PostCode).HasMaxLength(10);   
        entityBuilder.Property(c => c.Country).HasMaxLength(50);
        entityBuilder.Property(c => c.PhoneNumber).HasMaxLength(15);
        entityBuilder.Property(c => c.Name).HasMaxLength(50);

        entityBuilder
            .ToTable("Customers")
            .SplitToTable(
                "PhoneNumbers",
                tableBuilder =>
                {
                    tableBuilder.Property(customer => customer.Id).HasColumnName("CustomerId");
                    tableBuilder.Property(customer => customer.PhoneNumber);
                })
            .SplitToTable(
                "Addresses",
                tableBuilder =>
                {
                    tableBuilder.Property(customer => customer.Id).HasColumnName("CustomerId");
                    tableBuilder.Property(customer => customer.Street);
                    tableBuilder.Property(customer => customer.City);
                    tableBuilder.Property(customer => customer.PostCode);
                    tableBuilder.Property(customer => customer.Country);
                });
    });

            //modelBuilder.Entity<Order>()
            //    .Property(o => o.Status)
            //    .HasConversion<string>();

            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.DetailedOrder)
            //    .WithOne()
            //    .HasForeignKey<DetailedOrder>(o => o.Id);
        }

    }
}
