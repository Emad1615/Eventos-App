using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController() : ControllerBase
    {
        private IConfiguration? _config;
        protected IConfiguration Config => _config
            ?? HttpContext.RequestServices.GetService<IConfiguration>()
            ?? throw new InvalidOperationException("Iconfiguration service unavailable");
        protected string GenerateJwtToken(UserApplication user)
        {
            var claims = new List<Claim>()
     {
         new Claim(ClaimTypes.NameIdentifier,user.Id),
         new Claim(ClaimTypes.Name,user.UserName!),
         new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
     };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(Config["Jwt:ExpireMinutes"]!));
            var token = new JwtSecurityToken(
                    issuer: Config["Jwt:Issure"],
                    audience: Config["Jwt:Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: cred
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
