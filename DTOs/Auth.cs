using System.ComponentModel.DataAnnotations;

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

        public LoggedUser(string token)
        {
            this.AccessToken = token;
        }
    }
}