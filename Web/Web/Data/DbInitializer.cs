using System;
using System.Linq;
using Web.Models;

namespace Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();
            
            if (!context.Songs.Any())
            {
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
            
            if (!context.Movies.Any())
            {
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
}