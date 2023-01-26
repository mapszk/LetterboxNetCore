using LetterboxNetCore.Models;

namespace LetterboxNetCore.DTOs
{
    public class UserReviewDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }

        public UserReviewDTO(User user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.Avatar = user.Avatar;
        }
    }

    public class UserDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }

        public UserDTO(User user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.Avatar = user.Avatar;
        }
    }
}