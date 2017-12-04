using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class SongServiceContext : DbContext
    {
        public SongServiceContext (DbContextOptions<SongServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Models.Song> Songs { get; set; }



    }
}
