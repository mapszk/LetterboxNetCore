using Microsoft.AspNetCore.Identity;

namespace LetterboxNetCore.Models
{
    public class User : IdentityUser
    {
        public List<Review>? Reviews { get; set; }
        public List<MovieLike>? MovieLikes { get; set; }
        public List<MovieWatchlist>? MovieWatchlist { get; set; }
    }
}