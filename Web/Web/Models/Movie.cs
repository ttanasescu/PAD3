using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        public string Title { get; set; }

        [Range(1, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Producer { get; set; }

        public string About { get; set; }
    }
}