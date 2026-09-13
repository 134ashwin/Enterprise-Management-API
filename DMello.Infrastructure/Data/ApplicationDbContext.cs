using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DMello.Domain.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace DMello.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor passing configuration options to the base DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add your DbSets here (tables)
        public DbSet<User> Users => Set<User>();
        public DbSet<SalesOrdersModel> SalesOrders => Set<SalesOrdersModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations automatically from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Example: Define custom constraints, indexes, or relationships directly if needed
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Configure SalesOrder indexes and constraints
            modelBuilder.Entity<SalesOrdersModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderNo); // Ensures OrderNo cannot be duplicated
                entity.Property(e => e.OrderNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MainSku).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Customer).IsRequired().HasMaxLength(150);
            });
        }
    }
}
