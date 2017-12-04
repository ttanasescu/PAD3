using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public int Rating { get; set; }
        public string Lyrics { get; set; }
    }
}
