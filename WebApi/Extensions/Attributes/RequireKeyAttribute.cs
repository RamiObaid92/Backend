using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Attributes;

// Använde AI för att hjälpa mig att använda API nycklar på ett bättre sätt om jag vill använda både Admin och User API nycklar.
// Istället för att använda separata attribut filer för varje nyckel.

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequireKeyAttribute(params string[] keyNames) : Attribute, IAsyncActionFilter
{
    private readonly string[] _keyNames = keyNames;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        foreach (var keyName in _keyNames)
        {
            var expectedKey = config[$"SecretKeys:{keyName}"];

            if (string.IsNullOrEmpty(expectedKey))
                continue;

            var headerName = keyName switch
            {
                "AdminKey" => "X-ADM-API-KEY",
                "UserKey" => "X-USER-API-KEY",
                _ => $"X-{keyName.ToUpper()}"
            };

            if (context.HttpContext.Request.Headers.TryGetValue(headerName, out var providedKey) &&
                string.Equals(providedKey, expectedKey, StringComparison.Ordinal))
            {
                await next();
                return;
            }
        }

        context.Result = new UnauthorizedObjectResult("Missing or invalid API key.");
    }
}
