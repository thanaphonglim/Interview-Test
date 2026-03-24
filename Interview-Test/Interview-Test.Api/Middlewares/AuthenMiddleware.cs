using System.Security.Cryptography;
using System.Text;

namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    private const string hashedKey = "ccc50d1c6b496981235a62c8bce7963f9f8090db5745fa5a226f0b3773715a1d7c8d9b665e9a672a8f08cb36d481b8a5f6f504a433156076b186a1361cfebc5a";
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"];
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        using var sha512 = SHA512.Create();
        var hash = BitConverter.ToString(sha512.ComputeHash(Encoding.UTF8.GetBytes(apiKeyHeader!))).Replace("-", "").ToLower();

        if (hash != hashedKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        await next(context);
    }
}