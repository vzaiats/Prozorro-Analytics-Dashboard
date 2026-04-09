using Microsoft.EntityFrameworkCore;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Models;

namespace ProzorroDataMining.Data.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tender> Tenders { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
