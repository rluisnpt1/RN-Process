using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RN_Process.WebClientApi
{
    public static class ConfigureSwagger
    {
        public static void SwaggerConfigure(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Rn File Proccess",
                    Description = "An project to process files in from many end points.",
                    Contact = new OpenApiContact()
                        {Name = "Robson Nascimento", Email = "robsonluisn@outlook.com"},
                    License = new OpenApiLicense
                    {
                        Name = "MIT"
                    },
                });
            });
        }
    }
}