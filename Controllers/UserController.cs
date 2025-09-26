using AuthDotNetApi.Dto;
using AuthDotNetApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuthDotNetApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
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
        public ActionResult<User> Login(UserDto userDto)
        {
            if (!user.Username.Equals(userDto.Username))
            {
                return BadRequest("User not found.");
            }

            if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password) == false)
            {
                return BadRequest("Wrong password");
            }

            return Ok("Login successful.");
        }
    }

}