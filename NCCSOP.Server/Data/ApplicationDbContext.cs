using NCCSOP.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace NCCSOP.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SOP> SOPs { get; set; }
        public DbSet<SOPItem> SOPItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category → SOPs relationship
            modelBuilder.Entity<SOP>()
                .HasOne(s => s.Category)
                .WithMany(c => c.SOPs)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
