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
using MagoTrader.Web.Data;
using MagoTrader.Core;
using MagoTrader.Data;
using MagoTrader.Core.Repositories;
using MagoTrader.Exchange.MercadoBitcoin.Public;
//using MagoTrader.Data.Repositories;
//using MagoTrader.Exchange.Repositories;

namespace MagoTrader.Web
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
            //-----------API Access------------------------------
            services.AddHttpClient<IFetchDataService,FetchDataService>(client => {
                client.BaseAddress = new Uri("https://www.mercadobitcoin.net/api/");
            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddScoped<IFetchDataService,FetchFakeDataService>();
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
