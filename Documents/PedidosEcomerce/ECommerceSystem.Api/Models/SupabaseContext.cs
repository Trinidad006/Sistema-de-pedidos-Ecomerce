using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Api.Models
{
    public class SupabaseContext : DbContext
    {
        public SupabaseContext(DbContextOptions<SupabaseContext> options) : base(options) { }

        public DbSet<Venta> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venta>().ToTable("ventas");
        }
    }
} 