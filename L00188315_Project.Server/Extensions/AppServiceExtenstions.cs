using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Caching.Memory;

namespace L00188315_Project.Server.Extensions
{
    public static class AppServiceExtenstions
    {
        /// <summary>
        /// Register services for the application
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<MemoryCache>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<ICacheService, CacheService>(); // singleton, as we only want 1 instance of the cache service
            services.AddSingleton<IKeyVaultService, KeyVaultService>(); // singleton, as we only want 1 instance of the cache service


            services.AddScoped<IRevolutService, RevolutService>();
            services.AddScoped<ITokenService, TokenService>();


            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.MaxDepth = 8;
            });

            return services;
        }
    }
}
