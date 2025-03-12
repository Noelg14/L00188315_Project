using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using System.Text.Json.Serialization;

namespace L00188315_Project.Server.Extensions
{
    public static class AppServiceExtenstions
    {
        /// <summary>
        /// Register services for the application
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();



            builder.Services.AddSingleton<ICacheService, CacheService>(); // singleton, as we only want 1 instance of the cache service
            builder.Services.AddScoped<IRevolutService, RevolutService>();

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.MaxDepth = 8;
            });

            return builder;
        }
    }
}
