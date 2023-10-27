using Filuet.Hardware.Dispensers.Abstractions.Models;
using Filuet.Infrastructure.Abstractions.Converters;
using MPT.Vending.API.Dto;
using System.Net.Http.Headers;
using System.Reflection;
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
        public async Task<Kiosk> GetKioskAsync(string uid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks?uid={uid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Kiosk>(result);
        }

        public async Task<Kiosk> AddKioskAsync(string kioskUid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks?uid={kioskUid}"), null);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Kiosk>(result);
        }

        public async Task AddOrUpdateKioskAsync(Kiosk kiosk)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(kiosk, _options), Encoding.UTF8, "application/json");
            await _client.PutAsync(new Uri(new Uri(_url), $"/api/kiosks"), httpContent);
        }

        public async Task<IEnumerable<Kiosk>> GetKiosksAsync()
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks/all"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Kiosk>>(result);
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

        public async Task SetCreditAsync(string kioskUid, int credit)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_url), $"/api/kiosks/credit?kioskUid={kioskUid}&credit={credit}"), null);
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
        public async Task<Product> GetProductAsync(string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/products?sku={sku}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(result);
        }

        public async Task PutProductAsync(Product product)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_url), "/api/products"), httpContent);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(IEnumerable<string> sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(new ProductRequest { Sku = sku }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_url), $"/api/products/"), httpContent);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(result);
        }

        public async IAsyncEnumerator<Product> GetProductsAsync(string filter, CancellationToken token)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            using HttpResponseMessage response = await _client.GetAsync(
                new Uri(new Uri(_url), $"/api/products/all?filter={filter ?? string.Empty}"),
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using Stream responseStream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            var products = JsonSerializer.DeserializeAsyncEnumerable<Product>(
                responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 128
                });

            await foreach (var p in products)
            {
                if (token.IsCancellationRequested)
                {
                    responseStream.Close();
                    break;
                }
                else yield return p;
            }
        }

        public async Task<IEnumerable<Product>> DisableProductAsync(string kioskUid, string sku)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.DeleteAsync(new Uri(new Uri(_url), $"/api/products/disable"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(result);
        }

        public async Task PutPicture(string sku, string picture)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(new ProductPictureUpdateRequest { Sku = sku, Picture = picture }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_url), $"/api/products/picture"), httpContent);
            await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region sessions
        public async Task CommitSessionsAsync(Session session)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(session, _options), Encoding.UTF8, "application/json");
            await _client.PutAsync(new Uri(new Uri(_url), "/api/products/session"), httpContent);
        }

        public async Task<IEnumerable<Session>> GetSessionsAsync(SessionsRequest request)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_url), "/api/products/sessions"), httpContent);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Session>>(result);
        }
        #endregion

        #region login
        public async Task<HttpResponseMessage> LoginAsync(LoginRequest request)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return await _client.PostAsync(new Uri(new Uri(_url), "/api/customers/login"), httpContent);
        }
        #endregion

        #region replenishment
        /// <summary>
        /// Kiosk Uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<PoG> GetPlanogramAsync(string uid)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/replenishment/planogram?uid={uid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PoG>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Kiosk Uid</param>
        /// <param name="planogram"></param>
        /// <returns></returns>
        public async Task PutPlanogramAsync(string uid, PoG planogram)
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(planogram), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_url), $"/api/replenishment/planogram?uid={uid}"), httpContent);
        }
        #endregion

        #region media
        public async Task<IEnumerable<AdMedia>> GetMediaAsync() {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/media"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<AdMedia>>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kioskUid">Kiosk Uid</param>
        /// <returns></returns>
        public async Task<IEnumerable<KioskMediaLink>> GetKioskMediaAsync(string kioskUid) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/media/kiosk?uid={kioskUid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<KioskMediaLink>>(result);
        }

        public async Task<string> UploadMediaFileAsync(string fileName, byte[] data) {
            using (var content = new MultipartFormDataContent()) {
                content.Add(new StreamContent(new MemoryStream(data)) {
                    Headers =
                    {
                        ContentLength = data.Length,
                        ContentType = new MediaTypeHeaderValue("text/plain")
                    }
                }, "File", fileName);

                var response = await _client.PostAsync(new Uri(new Uri(_url), $"/api/media/upload"), content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PutKioskMediaAsync(string kioskUid, IEnumerable<KioskMediaLink> resources) {
            var httpContent = new StringContent(JsonSerializer.Serialize(resources), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_url), $"/api/kiosks/media/{kioskUid}"), httpContent);
        }

        public async Task<byte[]> DownloadMediaAsync(string hash, string format) {
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_url), $"/api/media/find/{format}/{hash}"));
            Stream result = await response.Content.ReadAsStreamAsync();
            byte[] buffer = new byte[result.Length];
            await result.ReadAsync(buffer, 0, (int)result.Length);
            return buffer;
        }
        #endregion
    }
}