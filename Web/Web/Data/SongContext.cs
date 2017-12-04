using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class SongContext : DbContext
    {
        public SongContext (DbContextOptions<SongContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>()
                .ToTable(nameof(Song))
                .HasKey(song => song.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connection = @"Data Source=.;Initial Catalog=pad3;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connection);
        }
    }
}
