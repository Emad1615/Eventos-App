using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController() : ControllerBase
    {
        private IConfiguration? _config;
        private IMediator? _mediator;
        private AppDbContext _db;

        protected AppDbContext db => _db
            ?? HttpContext.RequestServices.GetService<AppDbContext>()
            ?? throw new InvalidOperationException("AppDbContext service unavailable");
        protected IMediator Mediator => _mediator
            ?? HttpContext.RequestServices.GetService<IMediator>()
            ?? throw new InvalidOperationException("IMediator  service unavailable ");
        protected IConfiguration Config => _config
            ?? HttpContext.RequestServices.GetService<IConfiguration>()
            ?? throw new InvalidOperationException("Iconfiguration service unavailable");

        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.Value is not null && result.IsSuccess) return Ok(result.Value);
            if (!result.IsSuccess && result.Status == 404) return NotFound();
            return BadRequest(result.Error);
        }

        protected string GenerateJwtToken(UserApplication user)
        {
            var claims = new List<Claim>(){
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

        protected RefreshToken GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            return new RefreshToken
            {
                Token = token,
                ExpiresDate = DateTime.UtcNow.AddDays(7),
                CreatedByIP = GetIpAddress(),
                CreatedDateTime = DateTime.UtcNow,
            };
        }

        protected string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"]!;
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()!;
        }
        protected void SetCookie(string token)
        {
            var cookieOption = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", token, cookieOption);
        }
    }
}
