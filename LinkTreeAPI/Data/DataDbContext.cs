using LinkTreeAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace LinkTreeAPI.Data
{
    public class DataDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Links> Links { get; set; }
        public DataDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
