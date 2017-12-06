using System.ComponentModel.DataAnnotations;

namespace Web.Helpers
{
    public class PagingQuery
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? Size { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}