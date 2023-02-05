using System.ComponentModel.DataAnnotations;
using LetterboxNetCore.Models;

namespace LetterboxNetCore.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email or username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    public class LoggedUser
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public UserDTO User { get; set; }

        public LoggedUser(string accessToken, string refreshToken, User user)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.User = new UserDTO(user);
        }
    }
}