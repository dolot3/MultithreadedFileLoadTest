using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FileLoadTest
{
    public class LoadContext : DbContext
    {
        public LoadContext() { }

        public DbSet<DataModel> DataModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\JMPS_SQL_SERVER;Database=Testing;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataModel>().HasKey(m => m.id);
        }
    }
}
