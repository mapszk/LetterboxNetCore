using LetterboxNetCore.Models;

namespace LetterboxNetCore.DTOs
{
    public class UserReviewDTO
    {
        public string? UserId { get; set; }
        public string? Avatar { get; set; }

        public UserReviewDTO(User user)
        {
            this.UserId = user.Id;
            this.Avatar = user.Avatar;
        }
    }
}