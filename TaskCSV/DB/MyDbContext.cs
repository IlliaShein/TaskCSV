using Microsoft.EntityFrameworkCore;
using TaskCSV.DB;

namespace DbLib.Models.EntityFramework
{
    public class MyDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.Salary)
                .HasPrecision(18, 2);
        }
    }
}
