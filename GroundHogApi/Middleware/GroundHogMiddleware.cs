using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

public class GroundHogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;

    public GroundHogMiddleware(RequestDelegate next, IDistributedCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract custom headers
        string header1 = context.Request.Headers["CustomHeader1"];
        string header2 = context.Request.Headers["CustomHeader2"];
        string header3 = context.Request.Headers["CustomHeader3"];

        // Read and compute SHA sum of the request body
        string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        string shaSum = ComputeShaSum(requestBody);

        // Check if headers and SHA sum have been seen before
        string cacheKey = $"{header1}_{header2}_{header3}_{shaSum}";
        string key = await _cache?.GetStringAsync(cacheKey );

        if (key != null)
        {
            // Set to cache with an expiration time
            await _cache.SetStringAsync(cacheKey, requestBody );

            // Reset the request body stream position for further processing in the pipeline
            context.Request.Body.Position = 0;

            await _next(context);
        }
        else
        {
            // Handle the duplicate request as needed
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync("Duplicate request detected.");
        }
    }

    private static string ComputeShaSum(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
