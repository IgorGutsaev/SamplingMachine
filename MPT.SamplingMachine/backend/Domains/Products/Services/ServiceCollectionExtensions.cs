using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Infrastructure;
using MPT.Vending.Domains.Products.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Products.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="setupAction"></param>
        /// <param name="connectionString">if cs is empty then show demo data</param>
        /// <returns></returns>
        public static IServiceCollection AddCatalog(this IServiceCollection serviceCollection, Action<IServiceProvider, IProductService> setupAction, string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IProductService>(sp => {
                DemoProductService result = new DemoProductService();
                setupAction?.Invoke(sp, result);
                return result;
            }) : serviceCollection.AddDbContext<CatalogDbContext>((sp, options) => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
#endif
            }, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddTransient<ProductRepository>()
            .AddTransient<ProductLocalizationRepository>()
            .AddTransient<KioskProductLinkRepository>()
            .AddTransient<PictureRepository>()
            .AddTransient<IProductService>(sp => {
                ProductService result = new ProductService(sp.GetRequiredService<ProductRepository>(),
                    sp.GetRequiredService<ProductLocalizationRepository>(),
                    sp.GetRequiredService<KioskProductLinkRepository>(),
                    sp.GetRequiredService<PictureRepository>());
                setupAction?.Invoke(sp, result);
                return result;
            });
    }
}