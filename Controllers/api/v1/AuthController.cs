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
        public async Task<ActionResult<LoggedUser>> SignIn([FromBody] UserLoginDTO userLoginDTO)
        {
            User user = await unitOfWork.UserRepository.FindByEmailOrUsername(userLoginDTO.UserName);
            if (user == null) return Problem("User doesn't exists", statusCode: (int)HttpStatusCode.BadRequest);
            var result = await signInManager.PasswordSignInAsync(user, userLoginDTO.Password, false, false);
            if (result.Succeeded)
            {
                string accessToken = CreateAccessToken(user);
                var refreshToken = new RefreshToken
                {
                    Active = true,
                    Expiration = DateTime.UtcNow.AddDays(Convert.ToInt32(configuration["Jwt:RefreshTokenExpireDays"])),
                    Value = Guid.NewGuid().ToString("N"),
                    Used = false,
                    UserId = user.Id
                };

                unitOfWork.RefreshTokenRepository.Add(refreshToken);
                await unitOfWork.SaveAsync();

                Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
                Response.Cookies.Append("X-Refresh-Token", refreshToken.Value, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
                var loggedUser = new LoggedUser(accessToken, refreshToken.Value, user);
                return Ok(loggedUser);
            }
            else return Problem("Wrong password", statusCode: (int)HttpStatusCode.BadRequest);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoggedUser>> RefreshToken()
        {
            var refreshTokenValue = Request.Cookies["X-Refresh-Token"];
            var refreshToken = await unitOfWork.RefreshTokenRepository.FindByValue(refreshTokenValue);

            if (refreshToken == null || refreshToken.Active == false || refreshToken.Expiration <= DateTime.UtcNow)
            {
                return Problem("Invalid credentials", statusCode: (int)HttpStatusCode.Forbidden);
            }

            if (refreshToken.Used)
            {
                var refreshTokens = await unitOfWork.RefreshTokenRepository.GetActiveTokensByUserId(refreshToken.UserId);
                foreach (var token in refreshTokens)
                {
                    token.Active = false;
                    token.Used = true;
                }
                await unitOfWork.SaveAsync();
                return Problem("Invalid credentials", statusCode: (int)HttpStatusCode.Forbidden);
            }

            refreshToken.Used = true;
            var user = await unitOfWork.UserRepository.Get(refreshToken.UserId);
            if (user == null)
            {
                return Problem(statusCode: (int)HttpStatusCode.Forbidden);
            }
            var accessToken = CreateAccessToken(user);
            var newRefreshToken = new RefreshToken
            {
                Active = true,
                Expiration = DateTime.UtcNow.AddDays(Convert.ToInt32(configuration["Jwt:RefreshTokenExpireDays"])),
                Value = Guid.NewGuid().ToString("N"),
                Used = false,
                UserId = user.Id
            };
            unitOfWork.RefreshTokenRepository.Add(newRefreshToken);
            await unitOfWork.SaveAsync();

            Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
            Response.Cookies.Append("X-Refresh-Token", refreshToken.Value, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
            var loggedUser = new LoggedUser(accessToken, newRefreshToken.Value, user);
            return Ok(loggedUser);
        }

        private string CreateAccessToken(User user)
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
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt:AccessTokenExpireMinutes"])),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}