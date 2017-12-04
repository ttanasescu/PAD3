using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Movie
    {
        public int Id { get; set; } 
        [Required]
        public string Title { get; set; } 
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Producer { get; set; }
        public string About { get; set; }
    }
}