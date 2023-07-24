using DataAccessLayer.DataObject;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Bill> Bills { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Rate> Rates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-74Q1NN4;Initial Catalog=SystemMarket;User ID=sa;Password=Hoang2k3.;TrustServerCertificate=True;Trusted_Connection=true");
        }
    }
}
