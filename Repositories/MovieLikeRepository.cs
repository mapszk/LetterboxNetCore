using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class MovieLikeRepository : Repository<MovieLike>, IMovieLikeRepository
    {
        public MovieLikeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<MovieLike?> GetLikeFromUserByMovieId(string userId, int movieId)
        {
            return await context.MovieLikes.FirstOrDefaultAsync(l => l.MovieId == movieId && l.UserId == userId);
        }

        public async Task<bool> LikeExists(string userId, int movieId)
        {
            return await context.MovieLikes.FirstOrDefaultAsync(l => l.MovieId == movieId && l.UserId == userId) != null;
        }
    }
}