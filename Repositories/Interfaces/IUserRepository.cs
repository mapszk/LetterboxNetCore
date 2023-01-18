using LetterboxNetCore.Models;
using Microsoft.AspNetCore.Identity;

namespace LetterboxNetCore.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUser(User user, string password);
        Task<User?> FindByEmailOrUsername(string emailOrUsername);
        Task<bool> ExistsByEmailOrUsername(string emailOrUsername);
    }
}