using Zzaia.Finance.Connector;
using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using Zzaia.Finance.EventManager;
using Zzaia.Finance.EventManager.Models;
using Zzaia.Finance.Exchange.Binance;
using Zzaia.Finance.Web.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Zzaia.Finance.Application.Adapter.Binance
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
            //----------- Exchange API Clients -------------------
            services.AddHttpClient();
            services.AddExchange(ExchangeName.Binance,
                    privateCredential => Configuration.Bind("Exchange:Binance:Private", privateCredential),
                    tradeCredential => Configuration.Bind("Exchange:Binance:Trade", tradeCredential));
            services.AddSingleton<IExchangeSelector, ExchangeSelector>();

            //----------- Data Event Connectors -------------------

            services.AddWebSocketConnector(options =>
            {
                options.ExchangeName = ExchangeName.Binance;
                options.DataIn = BinanceExchange.Information.Markets;
                options.DataOut = new List<Type> { typeof(OrderBook) };
            });

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.EnableAdaptiveSampling = false;
            });

            services.AddEventManager(options =>
            {
                options.PublishStrategy = PublishStrategy.ParallelNoWait;
            }, typeof(Startup).Assembly, typeof(EventManagerExtension).Assembly);

            services.AddGrpcEventCommunication(Configuration["DataEventManagerService"]);
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

        }
    }
}
