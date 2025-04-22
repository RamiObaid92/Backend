using System;
using System.Threading.Tasks;
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
            var defaultApiKey = _configuration["SecretKeys:UserKey"];
            if (string.IsNullOrEmpty(defaultApiKey)
                || !context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var providedApiKey)
                || !string.Equals(providedApiKey, defaultApiKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or missing API key.");
                return;
            }

            await _next(context);
        }
    }
}