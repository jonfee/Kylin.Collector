using Microsoft.EntityFrameworkCore;
using ProductCollector.Data.Entity;
using System.Configuration;

namespace ProductCollector.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            optionsBuilder.UseSqlServer(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //系统模块接口授权
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(p => p.CategoryID);
            });
        }

        public DbSet<Category> Category { get { return Set<Category>(); } }
    }
}
