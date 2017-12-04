using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>()
                .ToTable(nameof(Song))
                .HasKey(song => song.Id);

            modelBuilder.Entity<Movie>()
                .ToTable(nameof(Movie))
                .HasKey(movie => movie.Id);
        }
    }
}
