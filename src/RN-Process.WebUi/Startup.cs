using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RN_Process.Api.DataAccess.Entities;
using RN_Process.Api.DataAccess.Repositories;
using RN_Process.Api.DataAccess.Repositories.MongoDb;
using RN_Process.Api.Interfaces;
using RN_Process.Api.Models;
using RN_Process.Api.Services;
using RN_Process.DataAccess.MongoDb;
using RN_Process.Shared.Commun;

namespace RN_Process.WebUi
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
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddNewtonsoftJson(opt =>
            {
                // Web API configuration and services
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                opt.SerializerSettings.Formatting = Formatting.Indented;
            });


            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDbConnection:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDbConnection:Database").Value;
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparseable input
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });
            RegisterTypes(services);
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            SwaggerConfiguration(services);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RN Process V1");
                // c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }


        private void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           
            services.AddScoped<IRepositoryMongo<Organization>, MongoOrganizationRepository>();
            services.AddScoped<IRepositoryMongo<Term>, MongoTermRepository>();
            services.AddScoped<IRepositoryMongo<TermDetail>, MongoTermDetailRepository>();
            services.AddScoped<IRepositoryMongo<TermDetailConfig>, MongoTermDetailConfigRepository>();
            services.AddScoped<IRepositoryMongo<OrganizationFile>, MongoOrganizationFileRepository>();
            services.AddTransient<IValidatorStrategy<ContractOrganization>,DefaultValidatorStrategy<ContractOrganization>>();

            services.AddTransient<IContractOrganizationDataServices, ContractOrganizationServices>();
            services.AddTransient<IContractFileDataService, ContractFileDataService>();

        }

        private static void SwaggerConfiguration(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RN Process API",
                    Description = "A project for connect with many data source do take files and import to a database.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Robson Nascimento",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/rluisnpt1")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license")
                    }
                });


            });
        }
    }
}