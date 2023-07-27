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

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Rate> Rates { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    //optionsBuilder.UseSqlServer("Server=DESKTOP-74Q1NN4;Initial Catalog=SystemMarket;User ID=sa;Password=Hoang2k3.;TrustServerCertificate=True;Trusted_Connection=true");
        //    optionsBuilder.UseSqlServer("Server=LAPTOP-F38HNCQS;Initial Catalog=SystemMarket;User ID=sa;Password=123456;TrustServerCertificate=True;Trusted_Connection=true");
        //}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ghi đè cấu hình identityuser và tạo data bằng migrations
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });
            modelBuilder.Entity<Category>().HasData
                (
                    new Category { },
                    new Category { },
                    new Category { }
                );
            modelBuilder.Entity<Product>().HasData
                (
                    new Product { },
                    new Product { },
                    new Product { }
                );
        }
    }
}
