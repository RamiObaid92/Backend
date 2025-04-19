using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class UserKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var userKey = config["SecretKeys:UserKey"];

        if (!context.HttpContext.Request.Headers.TryGetValue("X-USER-API-KEY", out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Api-Key Missing");
            return;
        }

        if (string.IsNullOrEmpty(userKey) || !userKey.Equals(apiKey.ToString()))
        {
            context.Result = new UnauthorizedObjectResult("Api-Key Invalid");
            return;
        }

        await next();
    }
}
