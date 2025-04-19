namespace WebApi.Extensions.Middleware;

public class DefaultKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;
    private const string ApiKeyHeaderName = "X-ADM-API-KEY";

    public async Task InvokeAsync(HttpContext context)
    {
        var requiredKey = _configuration["SecretKeys:Default"];
        if (string.IsNullOrEmpty(requiredKey) || !context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey) ||
            !requiredKey.Equals(apiKey.ToString()))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Api-Key Missing or Invalid");
            return;
        }

        await _next(context);
    }
}
