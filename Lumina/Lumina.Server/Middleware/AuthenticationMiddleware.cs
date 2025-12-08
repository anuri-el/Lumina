namespace Lumina.Server.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Пропускаємо публічні ендпоінти
            if (context.Request.Path.StartsWithSegments("/api/images") && context.Request.Method == "GET")
            {
                await _next(context);
                return;
            }

            // Перевіряємо токен (спрощена версія)
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                _logger.LogWarning("Unauthorized request to {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Message = "Authorization header is required"
                });
                return;
            }

            // TODO: Реальна перевірка JWT токена
            var token = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
