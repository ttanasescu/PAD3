using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class MovieServiceContext : DbContext
    {
        public MovieServiceContext (DbContextOptions<MovieServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Models.Movie> Movies { get; set; }
    }
}
