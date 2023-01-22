using System.Security.Claims;
using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetterboxNetCore.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesApiController : ControllerBase
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 0;
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public MoviesApiController(UnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<MovieDetailsDTO>> GetById([FromRoute] string slug)
        {
            var movie = await unitOfWork.MoviesRepository.GetMovieDetailsBySlug(slug);
            if (movie == null)
                return NotFound();
            var mappedMovie = new MovieDetailsDTO(movie);
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

        [HttpPost("{movieSlug}/review")]
        public async Task<ActionResult<ReviewDTO>> CreateReview([FromRoute] string movieSlug, [FromBody] CreateReviewDTO createReviewDTO)
        {
            var movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return BadRequest("Movie doesn't exists");
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            var review = new Review(createReviewDTO);
            review.UserId = user.Id;
            review.MovieId = movie.Id;
            unitOfWork.ReviewsRepository.Add(review);
            await unitOfWork.SaveAsync();
            var mappedReview = new ReviewDTO(review);
            return Ok(mappedReview);
        }

        [HttpGet("{movieSlug}/review/{reviewId}")]
        public async Task<ActionResult<ReviewDTO>> GetReviewById([FromRoute] string movieSlug, [FromRoute] int reviewId)
        {
            var movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return NotFound("Movie doesn't exists");
            var review = await unitOfWork.ReviewsRepository.GetReviewDetails(reviewId);
            if (review == null)
                return NotFound("Review doesn't exists");
            var mappedReview = new ReviewDTO(review);
            return Ok(mappedReview);
        }

        [HttpPost("{movieSlug}/like")]
        public async Task<ActionResult> LikeMovie([FromRoute] string movieSlug)
        {
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            Movie movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return NotFound("Movie doesn't exists");
            var likeExists = await unitOfWork.MovieLikesRepository.LikeExists(user.Id, movie.Id);
            if (likeExists)
                return BadRequest("Movie is already liked");
            var movieLike = new MovieLike();
            movieLike.UserId = user.Id;
            movieLike.MovieId = movie.Id;
            unitOfWork.MovieLikesRepository.Add(movieLike);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost("{movieSlug}/remove-like")]
        public async Task<ActionResult> RemoveLike([FromRoute] string movieSlug)
        {
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            Movie movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return NotFound("Movie doesn't exists");
            var movieLike = await unitOfWork.MovieLikesRepository.GetLikeFromUserByMovieId(user.Id, movie.Id);
            if (movieLike == null)
                return NotFound();
            unitOfWork.MovieLikesRepository.Delete(movieLike);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost("{movieSlug}/add-to-watchlist")]
        public async Task<ActionResult> AddToWatchlist([FromRoute] string movieSlug)
        {
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            Movie movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return NotFound("Movie doesn't exists");
            bool isInWatchlist = await unitOfWork.MovieWatchlistRepository.MovieExistsInWatchlist(user.Id, movie.Id);
            if (isInWatchlist)
                return BadRequest("Movie is already in watchlist");
            var movieWatchlist = new MovieWatchlist();
            movieWatchlist.UserId = user.Id;
            movieWatchlist.MovieId = movie.Id;
            unitOfWork.MovieWatchlistRepository.Add(movieWatchlist);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost("{movieSlug}/remove-from-watchlist")]
        public async Task<ActionResult> RemoveFromWatchlist([FromRoute] string movieSlug)
        {
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            Movie movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null)
                return NotFound("Movie doesn't exists");
            var movieWatchlist = await unitOfWork.MovieWatchlistRepository.GetMovieFromUserByMovieId(user.Id, movie.Id);
            if (movieWatchlist == null)
                return NotFound();
            unitOfWork.MovieWatchlistRepository.Delete(movieWatchlist);
            await unitOfWork.SaveAsync();
            return Ok();
        }
    }
}