public class APIKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string apiHeaderName = "X-Api-Key";
    private readonly string apiKey;

    public APIKeyMiddleware(RequestDelegate next, AppSettings appSettings)
    {
        _next = next;
        apiKey = appSettings.API_KEY ?? "";
        Console.WriteLine(apiKey);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(apiHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        await _next(context);
    }
}