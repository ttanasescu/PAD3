using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext (DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .ToTable(nameof(Movie))
                .HasKey(movie => movie.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connection = @"Data Source=.;Initial Catalog=pad3;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connection);
        }
    }
}
