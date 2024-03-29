﻿using Microsoft.AspNetCore;
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
                 if (context.HostingEnvironment.IsProduction())
                 {
                     string kvName = Environment.GetEnvironmentVariable("kvName", EnvironmentVariableTarget.Process);

                     var secretClient = new SecretClient(
                         new Uri($"https://{kvName}.vault.azure.net/"),
                             new DefaultAzureCredential());
                         config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                      }
                 })
            .UseStartup<Startup>()
            .Build();
    }
}
