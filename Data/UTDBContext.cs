using Microsoft.EntityFrameworkCore;
using UT.Models;

namespace UT.Data
{
    public class UTDBContext : DbContext
    {
        public UTDBContext(DbContextOptions<UTDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        /*database context utilized by the repo */
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
