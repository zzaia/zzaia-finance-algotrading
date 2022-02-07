using MarketIntelligency.Application.DataEventManager.Services;
using MarketIntelligency.EventManager;
using MarketIntelligency.EventManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarketIntelligency.Application.DataEventManager
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEventManager(options =>
            {
                options.PublishStrategy = PublishStrategy.ParallelNoWait;
            }, typeof(Startup).Assembly, typeof(EventManagerExtension).Assembly);

            // Grpc
            services.AddGrpc();
            services.AddDaprClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DataEventManagerService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }
            });
        }
    }
}
