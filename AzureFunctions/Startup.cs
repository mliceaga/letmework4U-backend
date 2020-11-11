using Core.Interfaces;
using Core.Interfaces.Persistence;
using Infrastructure.AppSettings;
using Infrastructure.CosmosDbData.Extensions;
using Infrastructure.CosmosDbData.Repository;
// using Infrastructure.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

// add the FunctionsStartup assembly attribute that specifies the type name used during startup
[assembly: FunctionsStartup(typeof(AzureFunctions.Startup))]
namespace AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Use a singleton Configuration throughout the application
            services.AddSingleton<IConfiguration>(configuration);

            // Singleton instance. See example usage in SendGridEmailService: inject IOptions<SendGridEmailSettings> in SendGridEmailService constructor
            // services.Configure<SendGridEmailSettings>(configuration.GetSection("SendGridEmailSettings"));

            // if default ILogger is desired instead of Serilog
            services.AddLogging();

            //Register SendGrid Email
            // services.AddScoped<IEmailService, SendGridEmailService>();
            // Bind database-related bindings
            var cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            services.AddScoped<IJobApplicationRepository, jobJobApplicationRepository>();
        }
    }
}
