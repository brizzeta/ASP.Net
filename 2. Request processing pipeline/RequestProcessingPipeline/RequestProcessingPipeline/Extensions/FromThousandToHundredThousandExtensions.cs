using RequestProcessingPipeline.Middleware;

namespace RequestProcessingPipeline.Extensions
{
    public static class FromThousandToHundredThousandExtensions
    {
        public static IApplicationBuilder UseFromThousandToHundredThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromThousandToHundredThousandMiddleware>();
        }
    }
}
