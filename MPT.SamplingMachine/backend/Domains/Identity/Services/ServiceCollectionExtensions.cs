using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Identity.Infrastructure.Repositories;
using MPT.Vending.Domains.Kiosks.Infrastructure;
using MPT.Vending.Domains.Kiosks.Services;

namespace MPT.Vending.Domains.Identity.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalIdentity(this IServiceCollection serviceCollection, string connectionString)
            => string.IsNullOrWhiteSpace(connectionString) ?
            serviceCollection.AddTransient<IIdentityService, DemoIdentityService>()
            : serviceCollection.AddDbContext<IdentityDbContext>((sp, options) => {
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
#endif
            }, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddTransient<UserRepository>()
            .AddTransient<ClaimRepository>()
            .AddTransient<UserClaimRepository>()
            .AddTransient<IIdentityService, IdentityService>();
    }
}