using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthDotNetApi.Dto;
using AuthDotNetApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthDotNetApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IConfiguration configuration) : ControllerBase
    {
        public static User user = new User();


        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            user.Username = userDto.Username;
            user.Password = hashedPassword;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto userDto)
        {
            if (!user.Username.Equals(userDto.Username))
            {
                return BadRequest("User not found.");
            }

            if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password) == false)
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var secret = configuration.GetValue<string>("AppSettings:Token");

            if (string.IsNullOrEmpty(secret)) throw new InvalidOperationException("JWT secret is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}