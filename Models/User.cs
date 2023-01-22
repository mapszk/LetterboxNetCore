using LetterboxNetCore.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LetterboxNetCore.Models
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<MovieLike>? MovieLikes { get; set; }
        public List<MovieWatchlist>? MovieWatchlist { get; set; }

        public User() { }

        public User(UserRegisterDTO userRegisterDTO)
        {
            this.Email = userRegisterDTO.Email;
            this.UserName = userRegisterDTO.UserName;
        }
    }
}