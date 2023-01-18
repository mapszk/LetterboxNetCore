using System.ComponentModel.DataAnnotations;

namespace LetterboxNetCore.DTOs
{
    public class CreateMovieDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Cover { get; set; }
        [Required]
        public string Director { get; set; }
    }
}