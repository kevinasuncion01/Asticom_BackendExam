using Asticom_BackendExam.Models.DbModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Asticom_BackendExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController: ControllerBase
    {
        private readonly UserManager<AdminInfo> _userManager;
        private readonly SignInManager<AdminInfo> _signInManager;
        private readonly IConfiguration _configuration;

        public AdminController(UserManager<AdminInfo> userManager,
            SignInManager<AdminInfo> signInManager, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    // Get the roles for the user
                    var roles = await _userManager.GetRolesAsync(user);

                    //get token
                    var key = Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    List<Claim> claims = new List<Claim>();
                    roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    var descriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(descriptor);
                    var _token = tokenHandler.WriteToken(token);
                    var results = new
                    {
                        token = _token,
                        expiration = token.ValidTo
                    };

                    return Created("", results);
                }

            }

            return BadRequest("Login Failed");
        }

        public class LoginModel
        {
            public string UserName { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
    }
}
