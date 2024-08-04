using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("")]
public class AuthController : Controller
{
    private readonly VisitExclusionConfiguration _visitExclusionConfiguration;

    public AuthController(IOptions<VisitExclusionConfiguration> visitExclusionConfiguration)
    {
        _visitExclusionConfiguration = visitExclusionConfiguration.Value;
    }

    [Authorize(AuthenticationSchemes = "AdminAuth")]
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

        HttpContext.Response.Cookies.Append(
            "ExcludeVisit",
            _visitExclusionConfiguration.ExcludeVisitCookieValue,
            new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.None
            }
        );

        return Ok("Admin login successful.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok("Admin logout successful.");
    }
}
