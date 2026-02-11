namespace EPM_Blogger.API.Middlewares
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _config = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the current endpoint has [Authorize]
            var endpoint = context.GetEndpoint();
            var hasAuthorize = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null;

            if (hasAuthorize)
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Missing or invalid Authorization header.");
                    return;
                }

                var token = authHeader["Bearer ".Length..].Trim();

                try
                {
                    var key = _config["Jwt:Key"];
                    var issuer = _config["Jwt:Issuer"];
                    var audience = _config["Jwt:Audience"];

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParams = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "")),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    // Validate and attach claims principal
                    var principal = tokenHandler.ValidateToken(token, validationParams, out _);
                    context.User = principal; 
                }
                catch (SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Token expired.");
                    return;
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token.");
                    return;
                }
            }

            await _next(context);
        }
    }

    // Extension method for cleaner Startup/Program.cs usage
    public static class JwtValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtValidationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtValidationMiddleware>();
        }
    }
}
