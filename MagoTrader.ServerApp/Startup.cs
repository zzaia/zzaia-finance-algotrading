using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MagoTrader.Core;
using MagoTrader.Core.Repositories;
using MagoTrader.Services;
using MagoTrader.Data;
using MagoTrader.Exchange;
using MagoTrader.Web;
using MagoTrader.Core.Models;
using MagoTrader.Core.Exchange;

namespace MagoTrader.ServerApp
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
            //------- DB Context configuration --------
            services.AddDbContextPool<MagoTraderDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MagoTraderSQLDB"));
            });
            
            services.AddScoped<IUnitOfWork, UnitOfWork>(); //It has all implemented data repositories inside;
            /*
            //----------- Generic API Access-------------------
            services.AddHttpClient<IFetchDataService,FetchDataService>(client => {
                client.BaseAddress = new Uri("https://www.mercadobitcoin.net/api/");
            });
            */
            // Configure Exchange Client with ApiOptions
            services.AddExchangeClient(ExchangeNameEnum.MercadoBitcoin, 
                privateApiOptions =>
                {
                    privateApiOptions.SecretKey = Configuration["KeyVault:MercadoBitcoin:Private:SecretKey"];
                    privateApiOptions.ConnectionKey = Configuration["KeyVault:MercadoBitcoin:Private:ConnectionKey"];
                },
                tradeApiOptions =>
                {
                    tradeApiOptions.SecretKey = Configuration["KeyVault:MercadoBitcoin:Trade:SecretKey"];
                    tradeApiOptions.ConnectionKey = Configuration["KeyVault:MercadoBitcoin:Trade:ConnectionKey"];
                },
                c =>
                {
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestHeaders.Add("User-Agent", "HttpClient-MercadoBitcoinPublicApiClient-MagoTrader");
                });
            

            //services.AddHttpClient();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            //services.AddScoped<IFetchDataService,FetchFakeDataService>();
            services.AddScoped<IFetchDataService,FetchDataService>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
