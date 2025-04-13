using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeletedOrder> DeletedOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Order>()
            //   .HasOne<Customer>()
            //   .WithMany(c => c.Orders)
            //   .HasForeignKey(o => o.CustomerId);

            //modelBuilder.Entity<Order>()
            //    .HasOne<Product>()
            //    .WithMany(c => c.Orders)
            //    .HasForeignKey(o => o.ProductId);

            
        }
    }
}
