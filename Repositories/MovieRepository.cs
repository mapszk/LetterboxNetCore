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

        public async Task<(List<Movie>, int)> GetAllPaginated(string name, int pageNumber, int pageSize)
        {
            var search = context.Movies
                .Where(m => String.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower()));
            var result = await search
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (result, search.Count());
        }
    }
}