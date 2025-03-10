namespace mvcbasic.Data
{
    using Farm.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;

    public class MvcBasicDbContext : DbContext
    {
        public MvcBasicDbContext(DbContextOptions<MvcBasicDbContext> options) : base(options)
        {
        }
 
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }

    }
}