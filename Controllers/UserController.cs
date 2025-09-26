using AuthDotNetApi.Dto;
using AuthDotNetApi.Model;
using Microsoft.AspNetCore.Identity;
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
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, user.Password);

            user.Username = userDto.Username;
            user.Password = hashedPassword;

            return Ok(user);
        }
    }
}