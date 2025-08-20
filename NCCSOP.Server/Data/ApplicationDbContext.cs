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
    }
}
