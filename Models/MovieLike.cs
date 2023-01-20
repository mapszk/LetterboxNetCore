namespace LetterboxNetCore.Models
{
    public class MovieLike : BaseEntity
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}