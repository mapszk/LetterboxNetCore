using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class MovieWatchlistRepository : Repository<MovieWatchlist>, IMovieWatchlistRepository
    {
        public MovieWatchlistRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<MovieWatchlist?> GetMovieFromUserByMovieId(string userId, int movieId)
        {
            return await context.MovieWatchlist.FirstOrDefaultAsync(mw => mw.UserId == userId && mw.MovieId == movieId);
        }

        public async Task<bool> MovieExistsInWatchlist(string userId, int movieId)
        {
            return await context.MovieWatchlist.FirstOrDefaultAsync(mw => mw.UserId == userId && mw.MovieId == movieId) != null;
        }
    }
}