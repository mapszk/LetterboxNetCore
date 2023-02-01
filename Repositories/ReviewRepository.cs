using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Review?> GetReviewDetails(int id)
        {
            return await context.Reviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(List<Review>, int)> GetAllPaginated(int movieId, string content, int pageSize, int pageNumber)
        {
            var search = context.Reviews
                .Where(r => r.MovieId == movieId)
                .Where(r => String.IsNullOrEmpty(content) || r.Content.ToLower().Contains(content.ToLower()))
                .Include(r => r.User)
                .Include(r => r.Movie)
                .OrderByDescending(r => r.CreatedAt);
            var result = await search
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (result, search.Count());
        }
    }
}