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

        private readonly SamplingMachineApiClient _client;
        private readonly IMemoryCachingService _memCache;
        private readonly MemoryCacher _kioskCache;
        private readonly string _kioskUid;
    }
}