using System;
using System.Linq;
using Web.Models;

namespace Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SongContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Songs.Any())
            {
                return; // DB has been seeded
            }

            var song = new Song
            {
                Duration = TimeSpan.FromMinutes(3),
                Lyrics = "Skrrrraa",
                Rating = 5,
                Title = "Man's not hot"
            };
            context.Songs.Add(song);

            context.SaveChanges();
        }

        public static void Initialize(MovieContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Movies.Any())
            {
                return; // DB has been seeded
            }

            var movie = new Movie
            {
                Title = "Titanic",
                About = "Botas",
                Genre = "Drama",
                Producer = "James Cameron",
                Year = 1997
            };
            context.Movies.Add(movie);

            context.SaveChanges();
        }
    }
}