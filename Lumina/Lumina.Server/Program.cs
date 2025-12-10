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

            // ВАЖЛИВО: Налаштовуємо URL для сервера
            builder.WebHost.UseUrls("http://localhost:5155", "https://localhost:7001");

            // Додаємо DbContext - використовуємо InMemory замість SQLite для уникнення проблем
            builder.Services.AddDbContext<LuminaContext>(options =>
                options.UseInMemoryDatabase("LuminaDB"));

            // Реєструємо репозиторії
            builder.Services.AddScoped(typeof(Lumina.Core.Interfaces.IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<Lumina.Data.Interfaces.IImageRepository, ImageRepository>();
            builder.Services.AddScoped<Lumina.Data.Interfaces.ICollageRepository, CollageRepository>();
            builder.Services.AddScoped<Lumina.Data.Interfaces.IEffectRepository, EffectRepository>();

            // Реєструємо сервіси
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ICollageService, CollageService>();
            builder.Services.AddScoped<IEffectService, EffectService>();

            // Додаємо CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

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

            // Використовуємо вбудований CORS замість власного
            app.UseCors("AllowAll");

            app.UseMiddleware<ResponseCachingMiddleware>();
            // app.UseMiddleware<AuthenticationMiddleware>(); // Закоментовано для розробки

            // Стандартний middleware
            // app.UseHttpsRedirection(); // Закоментовано, щоб працював HTTP
            app.UseAuthorization();
            app.MapControllers();

            // Ініціалізуємо базу даних (для InMemory це просто створює контекст)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LuminaContext>();
                // InMemory DB не потребує EnsureCreated()
                Console.WriteLine(" Database initialized (InMemory)");
            }

            Console.WriteLine(" Lumina Server is starting...");
            Console.WriteLine(" Listening on: http://localhost:5155");
            Console.WriteLine(" Listening on: https://localhost:7001");
            Console.WriteLine(" Swagger UI: http://localhost:5155/swagger");
            Console.WriteLine(" Using InMemory Database (data will be lost on restart)");
            Console.WriteLine(" Ready to accept connections!");

            app.Run();
        }
    }
}
