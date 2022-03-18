using Core.Models;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CafeJWTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly ApplicationDBContext _context;
        public TokenController(IConfiguration configuration, ApplicationDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }



       /* private readonly UserManager<UserInfo> userManager;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserInfo userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = new UserInfo() { UserName = userDetails.UserName, Email = userDetails.Email, Password = userDetails.Password, CafeFavorito = userDetails.CafeFavorito };
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);


            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            return Ok(new { Message = "User Registration Successful" });
        }
       */


        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            if (userInfo != null && userInfo.UserName != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.UserName, userInfo.Password);
                if (user != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                    new Claim("Id",user.UserId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Password", user.Password)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(20),
                            signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }


            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<UserInfo> GetUser(string userName, string pass)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == pass);
        }
    }
}
