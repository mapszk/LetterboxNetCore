namespace LetterboxNetCore.Models
{
    public class RefreshToken : BaseEntity
    {
        public string? Value { get; set; }
        public bool Active { get; set; }
        public DateTime Expiration { get; set; }
        public bool Used { get; set; }
        public User? User { get; set; }
        public string? UserId { get; set; }
    }
}