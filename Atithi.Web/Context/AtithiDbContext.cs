using Atithi.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Atithi.Web.Context
{
    public class AtithiDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; } // Category table
        public DbSet<Menu> Menus { get; set; } // Menu table

        public AtithiDbContext(DbContextOptions<AtithiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship between Category and Menu
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Category) // A Menu has one Category
                .WithMany(c => c.Menus) // A Category can have many Menus
                .HasForeignKey(m => m.CategoryId) // The foreign key in Menu pointing to Category
                .OnDelete(DeleteBehavior.Restrict); // Specify delete behavior

            modelBuilder.Entity<Category>()
                .HasIndex(category => category.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Order>()
           .HasMany(o => o.OrderItems) // An Order has many OrderItems
           .WithOne(oi => oi.Order) // An OrderItem has one Order
           .HasForeignKey(oi => oi.OrderId) // The foreign key in OrderItem pointing to Order
           .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure the one-to-many relationship between Menu and OrderItem
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.OrderItems) // A Menu has many OrderItems
                .WithOne(oi => oi.Menu) // An OrderItem has one Menu
                .HasForeignKey(oi => oi.MenuItemId) // Foreign key in OrderItem pointing to Menu
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete on Menu
        }
    }
}

