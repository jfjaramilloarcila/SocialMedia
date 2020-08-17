using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticaion(UserLogin login)
        {
            //if it is a valid user
            if (IsValidUser(login))
            {
                var token = GenerateToken();
                return Ok(new { token });
            }
            return NotFound();
        }

        private bool IsValidUser(UserLogin login)
        {
            return true;
        }

        private string GenerateToken()
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var singningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(singningCredentials);

            //Claims Info que se quiere agregar en el cuerpo del mensaje  para poder crear paload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Juan Felipe"),
                new Claim(ClaimTypes.Email, "jjaramillo01@hotmail.com"),
                new Claim(ClaimTypes.Role, "Administrador"),
            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                 _configuration["Authentication:Audience"],
                 claims,
                 DateTime.Now,
                 DateTime.UtcNow.AddMinutes(2)
            );

            //Token
            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
