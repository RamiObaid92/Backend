using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AdminKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var adminKey = config["SecretKeys:Admin"];

        if (!context.HttpContext.Request.Headers.TryGetValue("X-ADM-API-KEY", out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Api-Key Missing");
            return;
        }

        if (string.IsNullOrEmpty(adminKey) || !adminKey.Equals(apiKey.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedObjectResult("Api-Key Invalid");
            return;
        }

        await next();
    }
}