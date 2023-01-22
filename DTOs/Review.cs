using System.ComponentModel.DataAnnotations;
using LetterboxNetCore.Models;

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

        public ReviewDTO(Review review)
        {
            this.Id = review.Id;
            this.UserInfo = new UserReviewDTO(review.User);
            this.MovieInfo = new MovieDTO(review.Movie);
            this.Content = review.Content;
            this.CreatedAt = review.CreatedAt;
            this.UpdatedAt = review.UpdatedAt;
        }
    }
}