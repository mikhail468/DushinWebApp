using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using System;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace DushinWebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                IConfigurationRoot builtConfig = config.Build();
                string kvURL = builtConfig["KeyVaultConfig:kvURL"];
                string tenantId = builtConfig["KeyVaultConfig:tenantId"];
                string clientId = builtConfig["KeyVaultConfig:clientId"];
                string clientSecret = builtConfig["KeyVaultConfig:clientSecret"];

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                var secretClient = new SecretClient(
                            new Uri(kvURL), credential);
                        config.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions());
            })
                .UseStartup<Startup>()
                .Build();
    }
}
