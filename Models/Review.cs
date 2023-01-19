namespace LetterboxNetCore.Models
{
    public class Review : BaseEntity
    {
        public string? Content { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}