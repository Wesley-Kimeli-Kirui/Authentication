using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenAuthor.Entities;
using AuthenAuthor.Requests;
using AuthenAuthor.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthenAuthor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Dependency Injection - inject IUserInterface into UserController
        private readonly IUserInterface _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserController(IUserInterface userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<ActionResult<string>> RegisterUser(AddUser addUser)
        {
            var newUser = _mapper.Map<User>(addUser);

            // Passwords should be hashed and salted
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(addUser.Password);
            // newUser.Role = "Admin";
            var response = await _userService.RegisterUser(newUser);
            return CreatedAtAction(nameof(RegisterUser), response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUser loginUser)
        {
            var user = await _userService.GetUserByEmail(loginUser.Email);
            if (user == null)
            {
                return NotFound("Invalid Credentials, User not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                return BadRequest("Invalid Credentials");
            }
            // create a token
            var token = GenerateToken(user);

            return Ok(token);
        }
        private string GenerateToken(User user)
        {
            // key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("TokenSecurity:SecretKey")));
            // signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // payload
            List<Claim> claims = new();
            {
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
                claims.Add(new Claim("Sub", user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));

                // create tokens
                var tokenGenerated = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("TokenSecurity:Issuer"),
                    audience: _configuration.GetValue<string>("TokenSecurity:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );
                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(tokenGenerated);
            };

        }
    }
}