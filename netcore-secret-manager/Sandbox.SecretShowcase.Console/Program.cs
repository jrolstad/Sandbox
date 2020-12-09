using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Sandbox.SecretShowcase.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
             * First time setup: run this in the project directory:  dotnet user-secrets init
             * Add user secret: dotnet user-secrets set "SampleSecret" "12345"
             */
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>();

            var config = builder.Build();

            Console.WriteLine($"Environment Variable|OneDrive|{config["OneDrive"]}");
            Console.WriteLine($"JsonFile|Movies:ServiceApiKey|{config["Movies:ServiceApiKey"]}");
            Console.WriteLine($"UserSecret|SampleSecret|{config["SampleSecret"]}");



        }
    }
}
