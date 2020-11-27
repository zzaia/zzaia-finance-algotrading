using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace MarketIntelligency
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            /*   // Key Vaoult Configuration 
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    var env = hostContext.HostingEnvironment;
                    if (env.IsProduction() || env.IsStaging() || env.IsDevelopment())
                    {
                        IConfigurationRoot configRoot = configBuilder.Build();

                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        KeyVaultClient keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        string keyVaultConnectionString = $@"https://{configRoot["AzureKeyVaultName"]}.vault.azure.net";
                        configBuilder.AddAzureKeyVault(
                            keyVaultConnectionString, keyVaultClient, new DefaultKeyVaultSecretManager());
                    }
                })
            */
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
