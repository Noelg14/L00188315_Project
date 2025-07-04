﻿using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace L00188315_Project.Server.Extensions
{
    /// <summary>
    /// Extension methods for configuring Swagger in the application.
    /// </summary>
    public static class SwaggerServiceExtentsion
    {
        /// <summary>
        /// Configures Swagger for the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } },
                };
                c.AddSecurityRequirement(securityRequirement);
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = "Openbanking Dashboard API", Version = "v1" }
                );
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }

        /// <summary>
        /// Enables the Swagger UI and the Swagger JSON endpoint
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
