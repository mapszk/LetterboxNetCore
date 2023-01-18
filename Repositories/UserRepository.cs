using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using LetterboxNetCore.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LetterboxNetCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public Task<bool> ExistsByEmailOrUsername(string emailOrUsername)
        {
            throw new NotImplementedException();
        }

        public Task<User?> FindByEmailOrUsername(string emailOrUsername)
        {
            throw new NotImplementedException();
        }
    }
}