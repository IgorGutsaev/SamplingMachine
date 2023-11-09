using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Infrastructure;
using MPT.Vending.Domains.Kiosks.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="setupAction"></param>
        /// <param name="getProducts"></param>
        /// <param name="connectionString">if cs is empty then show demo data</param>
        /// <returns></returns>
        public static IServiceCollection AddKiosk(this IServiceCollection serviceCollection,
            Action<IKioskService> setupAction,
            Func<IEnumerable<string>, IEnumerable<Product>> getProducts,
            string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IKioskService>(sp => {
                DemoKioskService result = new DemoKioskService();
                setupAction?.Invoke(result);
                return result;
            }) : serviceCollection.AddDbContext<KioskDbContext>((sp, options) => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
#endif
            }, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddTransient<KioskRepository>()
            .AddTransient<KioskSettingsRepository>()
            .AddTransient<KioskProductLinkViewRepository>()
            .AddTransient<IKioskService>(sp => { KioskService result = new KioskService(sp.GetRequiredService<KioskRepository>(), 
                sp.GetRequiredService<KioskSettingsRepository>(),
                sp.GetRequiredService<KioskProductLinkViewRepository>(),
                getProducts);
                setupAction?.Invoke(result);
                return result;
            });
    }
}