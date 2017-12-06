using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class PagingQuery
    {
        [Range(1, int.MaxValue)]
        public int Size { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}