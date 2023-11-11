using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPT.Vending.Domains.Advertisement.Abstractions;
using MPT.Vending.Domains.Advertisement.Infrastructure;
using MPT.Vending.Domains.Advertisement.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Advertisement.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="connectionString">if cs is empty then show demo data</param>
        /// <returns></returns>
        public static IServiceCollection AddAdvertisement(this IServiceCollection serviceCollection, string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IMediaService, DemoMediaService>() : 
            serviceCollection.AddDbContext<AdvertisementDbContext>((sp, options) => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
#endif
            }, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddTransient<AdMediaRepository>()
            .AddTransient<KioskMediaLinkRepository>()
            .AddTransient<KioskMediaLinkViewRepository>()
            .AddTransient<IMediaService, MediaService>();
    }
}