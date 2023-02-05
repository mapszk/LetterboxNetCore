using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetterboxNetCore.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<RefreshToken?> FindByValue(string tokenValue)
        {
            return await this.context.RefreshTokens.FirstOrDefaultAsync(t => t.Value == tokenValue);
        }

        public async Task<List<RefreshToken>> GetActiveTokensByUserId(string userId)
        {
            return await this.context.RefreshTokens
                .Where(t => t.Active && t.Used == false && t.UserId == userId)
                .ToListAsync();
        }
    }
}