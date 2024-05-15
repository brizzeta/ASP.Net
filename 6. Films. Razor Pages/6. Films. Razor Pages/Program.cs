using Microsoft.EntityFrameworkCore;
using _6._Films._Razor_Pages.Model;
using _6._Films._Razor_Pages.Repository;

var builder = WebApplication.CreateBuilder(args);
// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<FilmContext>(options => options.UseSqlServer(connection));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
