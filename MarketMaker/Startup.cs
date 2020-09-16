using MarketMaker.Core;
using MarketMaker.Core.Exchange;
using MarketMaker.Core.Repositories;
using MarketMaker.Data;
using MarketMaker.Exchange;
using MarketMaker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: UserSecretsId("dc5b4f9c-8b0e-2hg9-9813-c86ce80c39e6")]
namespace MarketMaker.ServerApp
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

            //----------- Exchange API Client -------------------
            services.AddExchangeClient(ExchangeNameEnum.MercadoBitcoin,
                privateCredential => Configuration.Bind("Exchange:MercadoBitcoin:Private", privateCredential),
                tradeCredential => Configuration.Bind("Exchange:MercadoBitcoin:Trade", tradeCredential));

            services.AddRazorPages();
            services.AddServerSideBlazor();

            //services.AddScoped<IFetchDataService,FetchFakeDataService>();
            services.AddScoped<IFetchDataService, FetchDataService>();
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
