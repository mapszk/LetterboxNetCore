using AutoMapper;
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
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public MoviesController(UnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetById([FromRoute] int id)
        {
            var movie = await unitOfWork.MoviesRepository.Get(id);
            if (movie == null)
                return NotFound();
            var mappedMovie = mapper.Map<MovieDTO>(movie);
            return Ok(mappedMovie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDTO>> Create([FromBody] CreateMovieDTO createMovieDTO)
        {
            var movie = mapper.Map<Movie>(createMovieDTO);
            unitOfWork.MoviesRepository.Add(movie);
            await unitOfWork.SaveAsync();
            var created = mapper.Map<MovieDTO>(movie);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDTO>> Update([FromRoute] int id, [FromBody] CreateMovieDTO updateMovieDTO)
        {
            var movie = await unitOfWork.MoviesRepository.Get(id);
            if (movie == null)
                return NotFound();
            mapper.Map(updateMovieDTO, movie);
            await unitOfWork.SaveAsync();
            var updated = mapper.Map<MovieDTO>(movie);
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
    }
}