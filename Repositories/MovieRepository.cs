using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<bool> ExistsBySlug(string slug)
        {
            bool exists = await context.Movies.FirstOrDefaultAsync(m => m.Slug == slug) != null;
            return exists;
        }

        public async Task<Movie?> GetBySlug(string slug)
        {
            return await context.Movies.FirstOrDefaultAsync(m => m.Slug == slug);
        }

        public async Task<Movie?> GetMovieDetailsBySlug(string slug)
        {
            return await context.Movies
                .Include(m => m.MovieWatchlist)
                .Include(m => m.Reviews)
                .Include(m => m.Likes)
                .FirstOrDefaultAsync(m => m.Slug == slug);
        }
    }
}