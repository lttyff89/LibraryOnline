using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string BasicAuthenticationScheme = "BasicAuthentication";

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Obtener el encabezado de autenticación
            var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Basic"))
            {
                try
                {
                    var encodedCredentials = authorizationHeader.Substring("Basic ".Length).Trim();
                    var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                    var credentials = decodedCredentials.Split(':');

                    var username = credentials[0];
                    var password = credentials[1];

                    // Validar las credenciales (esto debe ser más seguro)
                    if (username == "admin" && password == "password")  // Ejemplo básico, deberías validar con una base de datos o un servicio real
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, "Admin")  // Aquí puedes agregar roles si lo deseas
                    };

                        var identity = new ClaimsIdentity(claims, BasicAuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, BasicAuthenticationScheme);

                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                    else
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
                    }
                }
                catch (Exception)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }
    }
}

