using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LetterboxNetCore.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;

        public AuthController(
            UnitOfWork unitOfWork,
            UserManager<User> userManager,
            IConfiguration configuration,
            SignInManager<User> signInManager
        )
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUp([FromBody] UserRegisterDTO userRegisterDTO)
        {
            bool userNameExists = await unitOfWork.UserRepository.ExistsByEmailOrUsername(userRegisterDTO.UserName);
            if (userNameExists) return Problem("Username already exists", statusCode: (int)HttpStatusCode.BadRequest);
            var user = new User(userRegisterDTO);
            var result = await unitOfWork.UserRepository.CreateUser(user, userRegisterDTO.Password);
            if (result.Succeeded) return Ok();
            else return Problem(result.Errors.First().Description, statusCode: (int)HttpStatusCode.BadRequest);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserLoginDTO userLoginDTO)
        {
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userLoginDTO.UserName);
            if (user == null) return Problem("User doesn't exists", statusCode: (int)HttpStatusCode.BadRequest);
            var result = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, false, false);
            if (result.Succeeded)
            {
                string token = CreateToken(user);
                var loggedUser = new LoggedUser(token);
                Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
                return Ok(loggedUser);
            }
            else return Problem("Wrong password", statusCode: (int)HttpStatusCode.BadRequest);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(configuration["Jwt:TokenExpireDays"])),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}