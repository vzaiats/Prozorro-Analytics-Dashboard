using Microsoft.OpenApi;
using System.Reflection;

namespace ProzorroDataMining.Api.Extensions
{
    public static class SwaggerExtensions
    {
        // Register Swagger services
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Prozorro Data Mining API",
                    Version = "v1",
                    Description = @"API for Prozorro analytics

#### Useful links:
- [Analytics Dashboard](http://localhost:3000)
- [Health UI dashboard](/health-ui)
- [Health endpoint](/health)"
                });

                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            return services;
        }

        // Configure Swagger middleware
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prozorro API v1");
                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
