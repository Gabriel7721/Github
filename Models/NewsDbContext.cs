using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MyEx1.Models
{
    public class NewsDbContext: DbContext
    {
        public NewsDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Admin>? Admins { get; set; }
        public DbSet<News>? News { get; set; }
    }
}
