using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace PersonalWebsite.Api.Authentication;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly BasicAuthConfiguration _configuration;

    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<BasicAuthConfiguration> configuration
    )
        : base(options, logger, encoder)
    {
        _configuration = configuration.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        try
        {
            var authValues = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]!);

            var isBasicAuthentication = "Basic".Equals(
                authValues.Scheme,
                StringComparison.InvariantCultureIgnoreCase
            );
            if (!isBasicAuthentication)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
            }

            var credentialBytes = Convert.FromBase64String(authValues.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            if (username != _configuration.Username || password != _configuration.Password)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
            }

            // success
            var claims = new[] { new Claim(ClaimTypes.Name, _configuration.Username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            return Task.FromResult(
                AuthenticateResult.Fail($"Error occurred during authentication: {ex.Message}")
            );
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = "Basic";
        Response.StatusCode = 401;

        return Task.CompletedTask;
    }
}
