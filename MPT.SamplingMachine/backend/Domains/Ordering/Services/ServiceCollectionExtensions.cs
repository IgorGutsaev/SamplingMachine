using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.Ordering.Infrastructure;
using MPT.Vending.Domains.Ordering.Infrastructure.Repositories;
using System.Text.Json;
using System.Text;
using Filuet.Infrastructure.Communication.Helpers;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Services
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
        public static IServiceCollection AddOrdering(this IServiceCollection serviceCollection, Action<IProductService> setupAction, string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IProductService>(sp => {
                DemoProductService result = new DemoProductService();
                setupAction?.Invoke(result);

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
            .AddTransient<TransactionRepository>()
            .AddTransient<CustomerRepository>()
            .AddTransient<IProductService>(sp => {
                ProductService result = new ProductService(sp.GetRequiredService<ProductRepository>(),
                    sp.GetRequiredService<ProductLocalizationRepository>(),
                    sp.GetRequiredService<KioskProductLinkRepository>(),
                    sp.GetRequiredService<PictureRepository>());
                setupAction?.Invoke(result);
                return result;
            })
            .AddTransient(sp => {
                ITransactionService result = string.IsNullOrWhiteSpace(connectionString) ?
                    new DemoTransactionService() :
                    new TransactionService(sp.GetRequiredService<TransactionRepository>(),
                        sp.GetRequiredService<CustomerRepository>(),
                        sp.GetRequiredService<ProductRepository>());

                result.OnNewTransaction += async (sender, e) => {
                    IConfiguration config = sp.GetRequiredService<IConfiguration>();
                    int index = 0;
                    while (true) {
                        string portalUrl = config[$"Portal:{index++}"];
                        if (!string.IsNullOrWhiteSpace(portalUrl)) {
                            HttpClient client = new HttpClient();
                            var httpContent = new StringContent(JsonSerializer.Serialize(new TransactionHookRequest { Message = HookHelpers.Encrypt(AzureKeyVaultReader.GetSecret("ogmentoportal-hook-secret"), JsonSerializer.Serialize(e)) }), Encoding.UTF8, "application/json");
                            try {
                                HttpResponseMessage response = await client.PostAsync(new Uri(new Uri(portalUrl), "/api/hook/transaction"), httpContent);
                            }
                            catch { }
                        }
                        else break;
                    }
                };

                return result;
            });
    }
}