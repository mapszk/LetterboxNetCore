using LetterboxNetCore.DTOs;

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
        public List<MovieWatchlist>? MovieWatchlist { get; set; }

        public Movie() { }
        public Movie(CreateMovieDTO createMovieDTO)
        {
            this.Name = createMovieDTO.Name;
            this.ReleaseYear = createMovieDTO.ReleaseYear;
            this.Description = createMovieDTO.Description;
            this.Cover = createMovieDTO.Cover;
            this.Director = createMovieDTO.Director;
        }

        public void Update(UpdateMovieDTO updateMovieDTO)
        {
            this.Name = updateMovieDTO.Name;
            this.ReleaseYear = updateMovieDTO.ReleaseYear;
            this.Description = updateMovieDTO.Description;
            this.Cover = updateMovieDTO.Cover;
            this.Director = updateMovieDTO.Director;
        }
    }
}