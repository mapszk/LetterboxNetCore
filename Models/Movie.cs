namespace LetterboxNetCore.Models
{
    public class Movie : BaseEntity
    {
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public string Director { get; set; }
    }
}