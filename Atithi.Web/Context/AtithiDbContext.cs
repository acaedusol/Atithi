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
                .HasIndex(category =>  category.CategoryName)
                .IsUnique();
        }
    }

}
