namespace Lumina.Server.Middleware
{
    public class ResponseCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseCachingMiddleware> _logger;
        private static readonly Dictionary<string, (byte[] Data, DateTime Expiry)> _cache = new();

        public ResponseCachingMiddleware(RequestDelegate next, ILogger<ResponseCachingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Кешуємо тільки GET запити
            if (context.Request.Method != "GET")
            {
                await _next(context);
                return;
            }

            var cacheKey = context.Request.Path.ToString();

            // Перевіряємо кеш
            if (_cache.TryGetValue(cacheKey, out var cached))
            {
                if (cached.Expiry > DateTime.UtcNow)
                {
                    _logger.LogInformation("Cache hit for {Path}", cacheKey);
                    await context.Response.Body.WriteAsync(cached.Data);
                    return;
                }
                else
                {
                    _cache.Remove(cacheKey);
                }
            }

            // Перехоплюємо відповідь
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Зберігаємо в кеш
            if (context.Response.StatusCode == 200)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseData = responseBody.ToArray();
                _cache[cacheKey] = (responseData, DateTime.UtcNow.AddMinutes(5));
                _logger.LogInformation("Cached response for {Path}", cacheKey);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }

            context.Response.Body = originalBodyStream;
        }
    }
}
