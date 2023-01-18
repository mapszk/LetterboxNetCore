using AutoMapper;
using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;
using LetterboxNetCore.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetterboxNetCore.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public AuthController(UnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult> SignUp([FromBody] UserRegisterDTO userRegisterDTO)
        {
            bool userNameExists = await unitOfWork.UserRepository.ExistsByEmailOrUsername(userRegisterDTO.UserName);
            if (userNameExists)
                return BadRequest($"A user with username '{userRegisterDTO.UserName}' already exists");
            var user = mapper.Map<User>(userRegisterDTO);
            var result = await unitOfWork.UserRepository.CreateUser(user, userRegisterDTO.Password);
            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors);
        }
    }
}