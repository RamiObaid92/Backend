using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class UseAdminApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string HEADER_NAME = "X-ADM-API-KEY";
        private const string CONFIG_KEY = "SecretKeys:AdminKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var adminApiKey = config[CONFIG_KEY];

            if (string.IsNullOrEmpty(adminApiKey)
                || !context.HttpContext.Request.Headers.TryGetValue(HEADER_NAME, out var providedApiKey)
                || !string.Equals(providedApiKey, adminApiKey, StringComparison.Ordinal))
            {
                context.Result = new UnauthorizedObjectResult("Invalid or missing Admin API key.");
                return;
            }

            await next();
        }
    }
}

// Använde AI för att hjälpa mig att använda API nycklar på ett bättre sätt om jag vill använda både Admin och User API nycklar.
// Istället för att använda separata attribut filer för varje nyckel.