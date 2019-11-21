using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RN_Process.Api.DataAccess;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Services;
using RN_Process.DataAccess.MongoDb;

namespace RN_Process.WebClientApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Configure the persistence in another layer
            //MongoDbPersistence.Configure();
            ConfigureSwagger.SwaggerConfigure(services);
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         

            app.UseHttpsRedirection();
  

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "RN File Process v1.0");
            });
        }


        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMongoContext, RnProcessMongoDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrganizationToContractOrganizationAdapter, OrganizationToContractOrganizationAdapter>();
            services.AddScoped<IContractOrganizationDataServices, ContractOrganizationServices>();
        }
    }
}
