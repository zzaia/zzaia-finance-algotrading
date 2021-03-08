using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.DataEventManager.ConnectorAggregate;
using MarketIntelligency.Exchange;
using MarketIntelligency.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: UserSecretsId("dc5b4f9c-8b0e-2hg9-9813-c86ce80c39e6")]
namespace MarketIntelligency.Application.SA0001
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //----------- Exchange API Client -------------------
            services.AddExchangeClient(ExchangeName.MercadoBitcoin,
                privateCredential => Configuration.Bind("Exchange:MercadoBitcoin:Private", privateCredential),
                tradeCredential => Configuration.Bind("Exchange:MercadoBitcoin:Trade", tradeCredential));

            services.AddScoped<IExchangeSelector, ExchangeSelector>();

            //----------- Data Event Connector -------------------
            services.AddConnector(options =>
                {
                    options.Name = ExchangeName.MercadoBitcoin.DisplayName;
                    options.TimeFrame = TimeFrame.m3;
                    options.DataIn = typeof(Market);
                    options.DataOut = typeof(OrderBook);
                });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.EnableAdaptiveSampling = false;
            });


            services.AddMediatR(typeof(Startup));

            // Grpc
            services.AddGrpc(opt =>
            {
                opt.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ControlService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }
            });
        }
    }
}