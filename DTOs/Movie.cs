using System.ComponentModel.DataAnnotations;

namespace LetterboxNetCore.DTOs
{
    public class CreateMovieDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Description max length is 50 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Release year is required")]
        public int ReleaseYear { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(300, ErrorMessage = "Description max length is 300 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Cover is required")]
        public string? Cover { get; set; }
        [Required(ErrorMessage = "Director is required")]
        public string? Director { get; set; }
    }
    public class UpdateMovieDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Description max length is 50 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Release year is required")]
        public int ReleaseYear { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(300, ErrorMessage = "Description max length is 300 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Cover is required")]
        public string? Cover { get; set; }
        [Required(ErrorMessage = "Director is required")]
        public string? Director { get; set; }
    }

    public class MovieDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ReleaseYear { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Cover { get; set; }
        public string? Director { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MovieDetailsDTO : MovieDTO
    {
        public int Likes { get; set; }
        public int Watchlist { get; set; }
        public int Reviews { get; set; }
    }
}