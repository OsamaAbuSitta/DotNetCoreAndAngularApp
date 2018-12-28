using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Dtos.Tofix;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public readonly IConfiguration _config ;

        public AuthController(IAuthRepository authRepository,IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto user)
        {
            if (await _authRepository.UserExists(user.Username))
                return BadRequest("User already exists");

            var userToCreate = new User { Username = user.Username };
            var createdUser = await _authRepository.Register(userToCreate, user.Password);

            return StatusCode(201);
        }

        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto user)
        {
          var userFromRepo = await _authRepository.Login(user.Username,user.Password);

          if(userFromRepo == null ) return Unauthorized();   
          
          var claims = new[]{
              new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
              new Claim(ClaimTypes.Name,userFromRepo.Username),
          };

          var tokenKey = System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value);
          var key = new  Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(tokenKey);
          var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);
          var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor{
                  Subject = new ClaimsIdentity(claims),
                  Expires = System.DateTime.Now.AddDays(1),
                  SigningCredentials = creds
          };

          var tokenHandler =new  System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
          var token = tokenHandler.CreateToken(tokenDescriptor);

          return Ok(
              new {
                  token = tokenHandler.WriteToken(token)
              }
              );
        }

    }
}