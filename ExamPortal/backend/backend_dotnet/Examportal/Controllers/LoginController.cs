using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Examportal.Auth;
using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Bcrypt = BCrypt.Net;

namespace Examportal.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

            ExamportalContext db = new ExamportalContext();

        [Route("/login")]
        [HttpPost]
        public IActionResult Post([FromBody] UserLoginCustomModel value)
        {
            var loginData = db.Users.FirstOrDefault(s => s.Email == value.Email);
            var loginStatus = Bcrypt.BCrypt.Verify(value.Password, loginData.Password);
            if (loginStatus)
            {
                String token = GenerateJSONToken(value);
                return Ok(new { token = token, accountType = loginData.AccountType });
            }
            else
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

        }
        [Route("/loggedIn")]
        [HttpGet]
        public IActionResult loggedInUser()
        {
            Dictionary<string, string> email = new Dictionary<string, string>();

            Authentication auth = new Authentication();
            email = auth.getAllClaims(HttpContext);
            String userEmail = "";
            foreach (KeyValuePair<string, string> em in email)
            {
                userEmail = em.Value;
                break;
            }
            var exams = db.Users.Where(e => e.Email == userEmail).ToList();
            return Ok(exams[0]);
        }

        private string GenerateJSONToken(UserLoginCustomModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
