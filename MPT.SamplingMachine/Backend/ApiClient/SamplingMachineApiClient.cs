using Filuet.Infrastructure.Abstractions.Converters;
using MPT.Vending.API.Dto;
using System.Text;
using System.Text.Json;

namespace MPT.SamplingMachine.ApiClient
{
    public class SamplingMachineApiClient : IDisposable
    {
        private readonly string _url;
        private readonly HttpClient _client;
        private JsonSerializerOptions _options;

        public SamplingMachineApiClient(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            _url = url;
            _client = new HttpClient();
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new CurrencyJsonConverter());
            _options.Converters.Add(new CountryJsonConverter());
            _options.Converters.Add(new LanguageJsonConverter());
            _options.Converters.Add(new N2JsonConverter());
        }

        public void Dispose()
        {
            _client.Dispose();
        }

     
        #region kiosks
        /// <summary>
        /// Get kiosk
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task<KioskDto> GetKioskAsync(string uid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks?uid={uid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<KioskDto>(result);
        }

        public async Task<KioskDto> AddKioskAsync(string kioskUid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks?uid={kioskUid}"), null);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<KioskDto>(result);
        }

        public async Task AddOrUpdateKioskAsync(KioskDto kiosk)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(kiosk, _options), Encoding.UTF8, "application/json");
            await _client.PutAsync(new Uri(new Uri(_url), $"/api/kiosks"), httpContent);
        }

        public async Task<IEnumerable<KioskDto>> GetKiosksAsync()
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks/all"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<KioskDto>>(result);
        }

        public async Task DisableProductLinkAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks/link/disable?kioskUid={kioskUid}&sku={sku}"), null);
        }

        public async Task EnableProductLinkAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks/link/enable?kioskUid={kioskUid}&sku={sku}"), null);
        }

        public async Task DeleteProductLinkAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.DeleteAsync(new Uri(new Uri(_url), $"/api/kiosks/link?kioskUid={kioskUid}&sku={sku}"));
        }

        public async Task AddProductLinkAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PutAsync(new Uri(new Uri(_url), $"/api/kiosks/link?kioskUid={kioskUid}&sku={sku}"), null);
        }

        public async Task SetCreditAsync(string kioskUid, string sku, int credit)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks/credit?kioskUid={kioskUid}&sku={sku}&credit={credit}"), null);
        }        
        
        public async Task SetMaxCountPerSession(string kioskUid, string sku, int maxCountPerSession)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks/limit?kioskUid={kioskUid}&sku={sku}&limit={maxCountPerSession}"), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task KioskEnableAsync(string uid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks/enable?uid={uid}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task KioskDisableAsync(string uid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks/disable?uid={uid}"));
        }
        #endregion

        #region products
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/products/all"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(result);
        }

        public async Task<IEnumerable<ProductDto>> DisableProductAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.DeleteAsync(new Uri(new Uri(_url), $"/api/products/disable"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(result);
        }
        #endregion
    }
}