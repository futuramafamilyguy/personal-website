using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("")]
public class AuthController : Controller
{
    public AuthController() { }

    [Authorize(AuthenticationSchemes = "AdminAuth")]
    [EnableRateLimiting("loginPolicy")]
    [HttpPost("login")]
    public IActionResult Login()
    {
        var claims = new[] { new Claim("Admin", "true") };
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        HttpContext
            .SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            )
            .Wait();

        return Ok("Admin login successful.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok("Admin logout successful.");
    }
}
