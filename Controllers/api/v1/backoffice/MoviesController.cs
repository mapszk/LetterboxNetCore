using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetterboxNetCore.Controllers
{
    [Route("api/v1/backoffice/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 0;
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public MoviesController(UnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetById([FromRoute] int id)
        {
            var movie = await unitOfWork.MoviesRepository.Get(id);
            if (movie == null)
                return NotFound();
            var mappedMovie = new MovieDTO(movie);
            return Ok(mappedMovie);
        }

        [HttpGet("get-all-paginated")]
        public async Task<ActionResult<PaginationDTO<MovieDTO>>> GetAllPaginated(
            [FromQuery] string? name,
            [FromQuery] int pageSize = DefaultPageSize,
            [FromQuery] int pageNumber = DefaultPageNumber
        )
        {
            var search = await unitOfWork.MoviesRepository.GetAllPaginated(name, pageNumber, pageSize);
            var mappedMovies = new List<MovieDTO>();
            foreach (var movie in search.Item1)
            {
                mappedMovies.Add(new MovieDTO(movie));
            }
            var result = new PaginationDTO<MovieDTO>
            (
                mappedMovies,
                search.Item2,
                Convert.ToInt32(Math.Ceiling((double)search.Item2 / (double)pageSize))
            );
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDTO>> Create([FromBody] CreateMovieDTO createMovieDTO)
        {
            var movie = new Movie(createMovieDTO);
            movie.Slug = await GenerateSlugAsync(createMovieDTO.Name, createMovieDTO.ReleaseYear, createMovieDTO.Director);
            unitOfWork.MoviesRepository.Add(movie);
            await unitOfWork.SaveAsync();
            var created = new MovieDTO(movie);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDTO>> Update([FromRoute] int id, [FromBody] UpdateMovieDTO updateMovieDTO)
        {
            var movie = await unitOfWork.MoviesRepository.Get(id);
            if (movie == null)
                return NotFound();
            movie.Update(updateMovieDTO);
            await unitOfWork.SaveAsync();
            var updated = new MovieDTO(movie);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieDTO>> Delete([FromRoute] int id)
        {
            var movie = await unitOfWork.MoviesRepository.Get(id);
            if (movie == null)
                return NotFound();
            unitOfWork.MoviesRepository.Delete(movie);
            await unitOfWork.SaveAsync();
            return NoContent();
        }

        private async Task<string> GenerateSlugAsync(string movieName, int releaseYear, string director)
        {
            string generatedSlug = movieName.ToLower().Replace(" ", "-");
            bool exists = await SlugExists(generatedSlug);
            if (!exists)
                return generatedSlug;
            generatedSlug = String.Concat(generatedSlug, $"-{Convert.ToString(releaseYear)}");
            exists = await SlugExists(generatedSlug);
            if (!exists)
                return generatedSlug;
            else
                return String.Concat(generatedSlug, $"-{director.ToLower().Replace(" ", "-")}");
        }

        private async Task<bool> SlugExists(string slug)
        {
            return await unitOfWork.MoviesRepository.ExistsBySlug(slug);
        }
    }
}