using Filuet.Hardware.Dispensers.Abstractions.Models;
using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using MPT.SamplingMachine.ApiClient;
using MPT.Vending.API.Dto;

namespace webapi.Services
{
    public class KioskService
    {
        const int KIOSK_CACHE_LIFETIME_DAYS = 31;

        public KioskService(SamplingMachineApiClient client, IMemoryCachingService memCache, string kioskUid) {
            if (string.IsNullOrWhiteSpace(kioskUid))
                throw new ArgumentException("Kiosk UID is mandatory");

            _client = client;
            _memCache = memCache;
            _kioskCache = _memCache.Get("kiosk");
            _kioskUid = kioskUid;
        }

        /// <summary>
        /// Retrieves current kiosk. Tries to get it from cache first, and if the cache is empty requests the API
        /// </summary>
        /// <returns></returns>
        public async Task<Kiosk> GetAsync() {
            Kiosk result = _kioskCache.Get<Kiosk>(_kioskUid);
            if (result == null) {
                result = await _client.GetKioskAsync(_kioskUid);
                result = result.PrepareForCommunication();
                _kioskCache.Set(_kioskUid, result, TimeSpan.FromDays(KIOSK_CACHE_LIFETIME_DAYS).TotalMilliseconds);
            }

            return result;
        }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest login)
            => await _client.LoginAsync(login);

        public async Task CommitTransactionAsync(Transaction session)
            => await _client.CommitTransactionsAsync(session);

        public void ClearCache()
            => _kioskCache.Clear();

        public async void DispenseAsync(object address)
            => await _client.DispenseAsync(address, _kioskUid);

        public async Task<IEnumerable<string>> GetDispensingList(IEnumerable<TransactionProductLink> products) {
            List<string> result = new List<string>();
            PoG planogram = await _client.GetPlanogramAsync(_kioskUid);
            foreach (var product in products) {
                for (int i = 0; i < product.Count; i++) {
                    PoGProduct poGProduct = planogram[product.Product.Sku];
                    PoGRoute? route = poGProduct.Routes.Where(x => x.Active.HasValue && x.Active.Value).OrderByDescending(x => x.Quantity).FirstOrDefault();
                    if (route != null) {
                        result.Add(route.Address);
                        route.Quantity--;
                    }
                }
            }

            return result;
        }

        private readonly SamplingMachineApiClient _client;
        private readonly IMemoryCachingService _memCache;
        private readonly MemoryCacher _kioskCache;
        private readonly string _kioskUid;
    }
}