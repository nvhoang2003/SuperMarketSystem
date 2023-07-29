using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.DTOs;
using DataAccessLayer.DataObject;

namespace SuperMarketSystem.Data;

public class MyDBContext : IdentityDbContext<ApplicationUser>
{
    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

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
        modelBuilder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.HasKey(e => e.UserId);
        });
        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasKey(e => e.UserId);
        });
        modelBuilder.Entity<IdentityUserToken<string>>(b =>
        {
            b.HasKey(e => e.UserId);
        });
        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasKey(e => e.UserId);
        });
        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasKey(e => e.UserId);
        });
        //modelBuilder.Entity<Category>().HasData
        //    (
        //        new Category { },
        //        new Category { },
        //        new Category { }
        //    );
        //modelBuilder.Entity<Product>().HasData
        //    (
        //        new Product { },
        //        new Product { },
        //        new Product { }
        //    );
        modelBuilder.Entity<IdentityUser>(b =>
        {
            b.Property(u => u.UserName).HasMaxLength(128);
            b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            b.Property(u => u.Email).HasMaxLength(128);
            b.Property(u => u.NormalizedEmail).HasMaxLength(128);
        });

        modelBuilder.Entity<IdentityUserToken<string>>(b =>
        {
            b.Property(t => t.LoginProvider).HasMaxLength(128);
            b.Property(t => t.Name).HasMaxLength(128);
        });
    }
}