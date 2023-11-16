using Microsoft.EntityFrameworkCore;
using World.Model;
using World;

namespace World.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Holyday> Holydays { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=internalDB.db");
        }
    }
}
