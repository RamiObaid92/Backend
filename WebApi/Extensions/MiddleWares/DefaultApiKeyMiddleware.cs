using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApi.Extensions.Middlewares
{
    public class DefaultApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private const string APIKEY_HEADER_NAME = "X-API-KEY";

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsOptions(context.Request.Method))
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path;
            if (path.StartsWithSegments("/api/auth/signin", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWithSegments("/api/auth/signup", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            bool hasAuthInfo = context.Request.Headers.ContainsKey("Authorization") || context.Request.Cookies.ContainsKey("jwt");
            if (hasAuthInfo)
            {
                await _next(context);
                return;
            }


            var defaultApiKey = _configuration["SecretKeys:UserKey"];

            if (string.IsNullOrEmpty(defaultApiKey))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var providedApiKey)
                || !string.Equals(providedApiKey.FirstOrDefault(), defaultApiKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or missing API key.");
                return;
            }

            await _next(context);
        }
    }
}