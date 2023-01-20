using System.Security.Claims;
using AutoMapper;
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
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public MoviesApiController(UnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<MovieDTO>> GetById([FromRoute] string slug)
        {
            var movie = await unitOfWork.MoviesRepository.GetBySlug(slug);
            if (movie == null)
                return NotFound();
            var mappedMovie = mapper.Map<MovieDTO>(movie);
            return Ok(mappedMovie);
        }

        [HttpPost("{movieId}/review")]
        public async Task<ActionResult<ReviewDTO>> CreateReview([FromRoute] int movieId, [FromBody] CreateReviewDTO createReviewDTO)
        {
            var movie = await unitOfWork.MoviesRepository.Get(movieId);
            if (movie == null)
                return BadRequest($"Movie with id {movieId} doesn't exists");
            string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userEmail);
            var review = mapper.Map<Review>(createReviewDTO);
            review.UserId = user.Id;
            review.MovieId = movie.Id;
            unitOfWork.ReviewsRepository.Add(review);
            await unitOfWork.SaveAsync();
            var mappedReview = mapper.Map<ReviewDTO>(review);
            return Ok(mappedReview);
        }

        [HttpGet("{movieId}/review/{reviewId}")]
        public async Task<ActionResult<ReviewDTO>> GetReviewById([FromRoute] int movieId, [FromRoute] int reviewId)
        {
            var movie = await unitOfWork.MoviesRepository.Get(movieId);
            if (movie == null)
                return NotFound($"Movie with id {movieId} doesn't exists");
            var review = await unitOfWork.ReviewsRepository.Get(reviewId);
            if (review == null)
                return NotFound($"Review with id {reviewId} doesn't exists");
            var mappedReview = mapper.Map<ReviewDTO>(review);
            return Ok(mappedReview);
        }

    }
}