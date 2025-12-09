using Lumina.Core.Interfaces;
using Lumina.Core.Services;
using Lumina.Data;
using Lumina.Data.Repositories;
using Lumina.Server.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Lumina.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо сервіси
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Додаємо DbContext
            builder.Services.AddDbContext<LuminaContext>(options =>
                options.UseSqlite("Data Source=lumina.db"));

            // Реєструємо репозиторії та сервіси
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ICollageService, CollageService>();
            builder.Services.AddScoped<IEffectService, EffectService>();

            // Додаємо логування
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            // Налаштовуємо Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Власні Middleware (порядок важливий!)
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<CorsMiddleware>();
            app.UseMiddleware<ResponseCachingMiddleware>();
            // app.UseMiddleware<AuthenticationMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
