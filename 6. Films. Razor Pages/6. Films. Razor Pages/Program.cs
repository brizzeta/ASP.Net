using Microsoft.EntityFrameworkCore;
using _6._Films._Razor_Pages.Model;
using _6._Films._Razor_Pages.Repository;

var builder = WebApplication.CreateBuilder(args);
// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<FilmContext>(options => options.UseSqlServer(connection));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
