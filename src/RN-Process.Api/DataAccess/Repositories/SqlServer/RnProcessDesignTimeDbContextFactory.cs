using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RN_Process.Api.DataAccess
{
    /// <summary>
    ///     For Integration tests
    /// </summary>
    public class RnProcessDesignTimeDbContextFactory : IDesignTimeDbContextFactory<RnProcessContext>
    {
        public RnProcessContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        public RnProcessContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        private RnProcessContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true);
            // .AddEnvironmentVariables();


            var config = builder.Build();

            var connstr = config.GetConnectionString("default");

            if (string.IsNullOrWhiteSpace(connstr))
                throw new InvalidOperationException(
                    "Could not find a connection string named 'default'.");
            return Create(connstr);
        }

        private RnProcessContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));

            var optionsBuilder =
                new DbContextOptionsBuilder<RnProcessContext>();

            Console.WriteLine("RnProcessDesignTimeDbContextFactory.Create(string): Connection string: {0}",
                connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            return new RnProcessContext(optionsBuilder.Options);
        }
    }
}