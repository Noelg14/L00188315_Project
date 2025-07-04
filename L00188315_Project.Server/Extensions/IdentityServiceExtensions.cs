﻿using System.Text;
using L00188315_Project.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace L00188315_Project.Server.Extensions
{
    /// <summary>
    /// Extension Class for the Identity services. Registers the ID services for the app.
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Add Identity services to the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                if (config.GetValue<string>("database:type") == "sqlite")
                {
                    opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
                }
                else
                {
                    opt.UseSqlServer(config.GetConnectionString("IdentityConnection"));
                }
            });

            services
                .AddIdentityCore<IdentityUser>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["token:key"]!)
                        ),
                        ValidIssuer = config["token:issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false,
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
