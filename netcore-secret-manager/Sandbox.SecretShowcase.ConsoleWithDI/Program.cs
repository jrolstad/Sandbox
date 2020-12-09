using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Sandbox.SecretShowcase.ConsoleWithDI
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
               .ConfigureAppConfiguration((hostContext, builder) =>
               {
                   builder.AddJsonFile($"appsettings.json", true, true);
                   builder.AddEnvironmentVariables();

                   if (hostContext.HostingEnvironment.IsDevelopment())
                   {
                       builder.AddUserSecrets<Program>();
                   }
               })
               .ConfigureServices((context, services) =>
               {
                   services.AddTransient<Application>();
               })
               .Build();


            var app = host.Services.GetService<Application>();

            app.Run();
        }
    }

    public class Application
    {
        private readonly IConfiguration _configuration;

        public Application(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Run()
        {
            Console.WriteLine($"ConsoleWithDI|Environment Variable|OneDrive|{_configuration["OneDrive"]}");
            Console.WriteLine($"ConsoleWithDI|JsonFile|Movies:ServiceApiKey|{_configuration["Movies:ServiceApiKey"]}");
            Console.WriteLine($"ConsoleWithDI|UserSecret|SampleSecret|{_configuration["SampleSecret"]}");
        }
    }

}
