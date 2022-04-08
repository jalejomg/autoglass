using Autoglass.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoglass.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
