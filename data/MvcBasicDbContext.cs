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

        public DbSet<Category> Category { get; set; }

        public DbSet<Cage> Cage { get; set; }
        public DbSet<Animal> Animal { get; set; }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Therapy> Therapy { get; set; }
        public DbSet<Deal> Deal { get; set; }



    }
}