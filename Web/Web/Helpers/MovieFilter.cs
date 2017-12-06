namespace Web.Helpers
{
    public class MovieFilter : PagingQuery
    {
        public int? Year { get; set; }
        public string Genre { get; set; }
        public string Producer { get; set; }
    }

    public class SongFilter : PagingQuery
    {
        public int? Rating { get; set; }
    }
}