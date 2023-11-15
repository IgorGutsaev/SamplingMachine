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
        public static IServiceCollection AddKiosk(this IServiceCollection serviceCollection,
            Action<IKioskService> kioskSetup,
            Action<IReplenishmentService> replenishmentSetup,
            Func<IEnumerable<string>, IEnumerable<Product>> getProducts,
            Func<IEnumerable<string>, Dictionary<string, IEnumerable<KioskMediaLink>>> getMedia,
            string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IKioskService>(sp => {
                DemoKioskService result = new DemoKioskService();
                kioskSetup?.Invoke(result);
                return result;
            }).AddTransient<IReplenishmentService>(sp => {
                DemoReplenishmentService result = new DemoReplenishmentService();
                replenishmentSetup?.Invoke(result);
                return result;
            })
            : serviceCollection.AddDbContext<KioskDbContext>((sp, options) => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
#endif
            }, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddTransient<KioskRepository>()
            .AddTransient<KioskSettingsRepository>()
            .AddTransient<KioskProductLinkViewRepository>()
            .AddTransient<PlanogramRepository>()
            .AddTransient<PlanogramViewRepository>()
            .AddTransient<IReplenishmentService>(sp => {
                ReplenishmentService result = new ReplenishmentService(sp.GetRequiredService<PlanogramRepository>(), sp.GetRequiredService<PlanogramViewRepository>());
                replenishmentSetup?.Invoke(result);
                return result;
            })
            .AddTransient<IKioskService>(sp => { KioskService result = new KioskService(sp.GetRequiredService<KioskRepository>(), 
                sp.GetRequiredService<KioskSettingsRepository>(),
                sp.GetRequiredService<KioskProductLinkViewRepository>(),
                sp.GetRequiredService<PlanogramRepository>(),
                sp.GetRequiredService<PlanogramViewRepository>(),
                getProducts, getMedia);
                kioskSetup?.Invoke(result);
                return result;
            });
    }
}