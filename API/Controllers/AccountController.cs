using API.DTOS;
using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class AccountController(SignInManager<UserApplication> signInManager) : BaseApiController
    {
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("is-Authenticated")]
        public ActionResult<bool> IsAuthenticated()
        {
            if (User?.Identity == null) return Ok(false);
            return Ok(User?.Identity?.IsAuthenticated);
        }
        [AllowAnonymous]
        [HttpPost("createAccount")]
        public async Task<ActionResult> CreateAccount(RegisterDTO register)
        {
            var user = new UserApplication()
            {
                UserName = register.Email,
                Email = register.Email,
                FullName = register.FullName,
                Geneder = register.Geneder,
                BirthDate = register.BirthDate,
                Bio = register.Bio,
            };
            var result = await signInManager.UserManager.CreateAsync(user, register.Password);
            if (result.Succeeded) return Ok();
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
            return ValidationProblem();
        }
        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return Unauthorized();
            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user == null) return NoContent();
            return Ok(new UserApplication
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                Geneder = user.Geneder,
                BirthDate = user.BirthDate,
                ImageUrl = user.ImageUrl,
            });
        }



        [AllowAnonymous]
        [HttpGet("login-jwt")]
        public async Task<ActionResult> Login(CredentialDTO credential)
        {
            var user = await signInManager.UserManager.FindByEmailAsync(credential.Email);
            if (user == null) return Unauthorized();
            if (!await signInManager.UserManager.CheckPasswordAsync(user, credential.Password)) return Unauthorized();
           
            //generate access token (JWT token)
            var token = GenerateJwtToken(user);
            
            // generate refresh token and  save it in database
            var refreshToken = GenerateRefreshToken();
            refreshToken.UserId=user.Id;
            await db.refreshTokens.AddAsync(refreshToken);
            await db.SaveChangesAsync();
            // add refresh token to Cookie
            SetCookie(refreshToken.Token);

            return Ok(new { token });
        }
 
        [HttpGet("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            var cookieToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(cookieToken)) return Unauthorized("No refresh Token");
            var refreshToken = await db.refreshTokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Token == cookieToken);
            if (refreshToken == null || !refreshToken.IsActive) return Unauthorized("Invalid refreshToken");

            refreshToken.RevokedDateTime = DateTime.UtcNow;
            refreshToken.RevokedByIP = GetIpAddress();

            var newRefreshToken = GenerateRefreshToken();

            refreshToken.ReplacedByToken = newRefreshToken.Token;

            newRefreshToken.UserId = UserId;

            await db.refreshTokens.AddAsync(newRefreshToken);

            await db.SaveChangesAsync();

            SetCookie(newRefreshToken.Token);
            var accessToken = GenerateJwtToken(refreshToken.User);
            return Ok(new { AccessToken = accessToken });
        }
        [HttpGet("revoke-refreshToken")]
        public async Task<ActionResult> Revoke()
        {
            var cookieToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(cookieToken)) return Unauthorized("No refresh token");
            var refrshToken = await db.refreshTokens.SingleOrDefaultAsync(x => x.Token == cookieToken);
            if (refrshToken == null || !refrshToken.IsActive) return Unauthorized("Invalid Refresh Token");
            refrshToken.RevokedDateTime = DateTime.UtcNow;
            refrshToken.RevokedByIP = GetIpAddress();
            await db.SaveChangesAsync();
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user-info-jwt")]
        public async Task<ActionResult<UserApplication>> GetUserInfoByJWT()
        {
            if (User.Identity?.IsAuthenticated == false) return Unauthorized();
            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user == null) return NoContent();
            return Ok(new UserApplication()
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                Geneder = user.Geneder,
                BirthDate = user.BirthDate,
                ImageUrl = user.ImageUrl,
            });
        }
    }
}
