using System.Net;
using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetterboxNetCore.Controllers
{
    [Route("api/v1/reviews")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewsApiController : ControllerBase
    {
        private const int DefaultPageSize = 5;
        private const int DefaultPageNumber = 0;
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public ReviewsApiController(UnitOfWork unitOfWork, UserManager<User> userManager, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpGet("{movieSlug}")]
        public async Task<ActionResult<PaginationDTO<ReviewDTO>>> GetAllPaginated(
            [FromRoute] string movieSlug,
            [FromQuery] string? content,
            [FromQuery] int pageSize = DefaultPageSize,
            [FromQuery] int pageNumber = DefaultPageNumber
        )
        {
            var movie = await unitOfWork.MoviesRepository.GetBySlug(movieSlug);
            if (movie == null) return Problem("Movie doesn't exists", statusCode: (int)HttpStatusCode.NotFound);
            var search = await unitOfWork.ReviewsRepository.GetAllPaginated(movie.Id, content, pageSize, pageNumber);
            var mappedReviews = new List<ReviewDTO>();
            foreach (var review in search.Item1)
            {
                mappedReviews.Add(new ReviewDTO(review));
            }
            var result = new PaginationDTO<ReviewDTO>
            (
                mappedReviews,
                search.Item2,
                Convert.ToInt32(Math.Ceiling((double)search.Item2 / (double)pageSize))
            );
            return Ok(result);
        }
    }
}