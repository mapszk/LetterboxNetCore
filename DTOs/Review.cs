using System.ComponentModel.DataAnnotations;

namespace LetterboxNetCore.DTOs
{
    public class CreateReviewDTO
    {
        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }
    }

    public class ReviewDTO
    {
        public int Id { get; set; }
        public UserReviewDTO? UserInfo { get; set; }
        public MovieDTO? MovieInfo { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}