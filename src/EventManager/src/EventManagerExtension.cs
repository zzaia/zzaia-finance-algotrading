using Zzaia.Finance.EventManager.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Zzaia.Finance.EventManager
{
    /// <summary>
    /// Extension methods for the Data Event Manager.
    /// </summary>
    public static class EventManagerExtension
    {
        /// <summary>
        /// Adds services and options for the data event manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="managerOptions">A delegate to configure the <see cref="EventManagerOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddEventManager(this IServiceCollection services,
             Action<EventManagerOptions> managerOptions, params Assembly[] assemblies)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (managerOptions is null)
            {
                throw new ArgumentNullException(nameof(managerOptions));
            }

            var eventManagerOptionsModel = new EventManagerOptions();
            managerOptions.Invoke(eventManagerOptionsModel);

            services.AddSingleton<ServiceFactory>(p => p.GetService);
            services.AddSingleton<IDataStreamSource, DataStreamSource>();

            services.AddTransient<IMediator, CustomMediator>((s) =>
            {
                var serviceFactory = (ServiceFactory)s.GetService(typeof(ServiceFactory));
                return new CustomMediator(serviceFactory, eventManagerOptionsModel.PublishStrategy);
            });
            services.AddMediatR(cfg => cfg.Using<CustomMediator>().AsSingleton(), assemblies);
            return services;
        }
    }
}
