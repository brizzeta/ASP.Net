using RequestProcessingPipeline.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // Добавляем сервисы сессии
var app = builder.Build();

app.UseSession();   // Добавляем middleware-компонент для работы с сессиями

// Добавляем middleware-компоненты в конвейер обработки запроса.
app.UseFromThousandToHundredThousand(); //1000-10000
app.UseFromHundredToThousand(); //100-1000
app.UseFromTwentyToHundred();// 20-100
app.UseFromElevenToNineteen();//11-19
app.UseFromOneToTen();//1-9

app.Run();
