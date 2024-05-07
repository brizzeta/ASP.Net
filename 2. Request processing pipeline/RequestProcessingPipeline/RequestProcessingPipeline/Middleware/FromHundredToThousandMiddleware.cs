namespace RequestProcessingPipeline.Middleware
{
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate? _next;
        public FromHundredToThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 100) await _next!.Invoke(context);
                else
                {
                    string[] hundreds = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };
                    if (number % 10 == 0)
                    {
                        // Выдаем окончательный ответ клиенту
                        context.Session.SetString("number", hundreds[number / 100 - 1]);
                    }
                    else
                    {
                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); // получим число от компонента FromOneToTenMiddleware
                        context.Session.SetString("number", hundreds[number / 100 - 1] + " hundred " + result);
                        // Выдаем окончательный ответ клиенту
                        // await context.Response.WriteAsync("Your number is " + hundreds[number / 100 - 1] + " " + result);
                    }
                }
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
}
