using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;

namespace LetterboxNetCore.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}