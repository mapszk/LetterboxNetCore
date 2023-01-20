namespace LetterboxNetCore.Models
{
    public class Movie : BaseEntity
    {
        public string? Name { get; set; }
        public int ReleaseYear { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Cover { get; set; }
        public string? Director { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<MovieLike>? Likes { get; set; }
    }
}