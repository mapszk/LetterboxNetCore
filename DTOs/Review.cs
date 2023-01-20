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
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public int MovieId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}