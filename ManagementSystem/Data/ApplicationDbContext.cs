using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManagementSystem.Models;

namespace ManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ManagementSystem.Models.Customer> Customer { get; set; } = default!;
        public DbSet<ManagementSystem.Models.Product> Product { get; set; } = default!;
        public DbSet<ManagementSystem.Models.Purchase> Purchase { get; set; } = default!;
        public DbSet<ManagementSystem.Models.PurchaseProduct> PurchaseProduct { get; set; } = default!;
        public DbSet<ManagementSystem.Models.Sale> Sale { get; set; } = default!;
        public DbSet<ManagementSystem.Models.SaleProduct> SaleProduct { get; set; } = default!;
        public DbSet<ManagementSystem.Models.Supplier> Supplier { get; set; } = default!;
    }
}
