using Microsoft.AspNetCore.Identity;

namespace LetterboxNetCore.Models
{
    public class User : IdentityUser
    {
        public List<Review>? Reviews { get; set; }
    }
}