using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Data;
using L00188315_Project.Infrastructure.Repositories;
using L00188315_Project.Infrastructure.Services;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace L00188315_Project.Server.Extensions
{
    public static class AppServiceExtenstions
    {
        /// <summary>
        /// Register the services for the app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAppServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddMemoryCache(opt =>
            {
                opt.ExpirationScanFrequency = TimeSpan.FromMinutes(1); // scan for expired items every minute
            });
            services.AddSingleton<MemoryCache>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<ICacheService, CacheService>(); // singleton, as we only want 1 instance of the cache service
            services.AddSingleton<IKeyVaultService, KeyVaultService>(); // singleton, as we only want 1 instance of the cache service

            services.AddScoped<IConsentRepository, ConsentRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IRevolutService, RevolutService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<OpenBankingMapper>();

            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNameCaseInsensitive = true;
                options.SerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.MaxDepth = 8;
            });

            return services;
        }
    }
}
