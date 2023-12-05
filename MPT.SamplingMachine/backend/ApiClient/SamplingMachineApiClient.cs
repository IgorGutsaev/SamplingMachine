using Filuet.Hardware.Dispensers.Abstractions.Models;
using Filuet.Infrastructure.Abstractions.Converters;
using Microsoft.Extensions.Caching.Memory;
using MPT.Vending.API.Dto;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace MPT.SamplingMachine.ApiClient
{
    public class SamplingMachineApiClient : IDisposable
    {
        private readonly SamplingMachineApiClientSettings _settings;
        private readonly HttpClient _client;
        private JsonSerializerOptions _options;
        private MemoryCache _memCache;

        public SamplingMachineApiClient(string url) {
            _settings = new SamplingMachineApiClientSettings { Url = url };
        }

        public SamplingMachineApiClient(Action<SamplingMachineApiClientSettings> setupAction) {
            _settings = new SamplingMachineApiClientSettings();
            setupAction?.Invoke(_settings);

            if (string.IsNullOrWhiteSpace(_settings.Url))
                throw new ArgumentNullException(nameof(_settings.Url));

            if (string.IsNullOrWhiteSpace(_settings.Email))
                throw new ArgumentNullException(nameof(_settings.Email));

            if (string.IsNullOrWhiteSpace(_settings.Password))
                throw new ArgumentNullException(nameof(_settings.Password));

            _client = new HttpClient();
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new CurrencyJsonConverter());
            _options.Converters.Add(new CountryJsonConverter());
            _options.Converters.Add(new LanguageJsonConverter());
            _options.Converters.Add(new N2JsonConverter());

            _memCache = new MemoryCache(new MemoryCacheOptions {
                SizeLimit = 1024 * 1000 * 1 // 1Mb
            });
        }

        private async Task<string> Token() {
            string token = _memCache.Get<string>("jwt")!;

            if (string.IsNullOrWhiteSpace(token)) {

                var httpContent = new StringContent(JsonSerializer.Serialize(new { _settings.Email, _settings.Password }, _options), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_settings.Url), $"/signin"), httpContent);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Authorization failed");

                token = await response.Content.ReadAsStringAsync();

                _memCache.Set("jwt", token, new MemoryCacheEntryOptions { Size = 1000, AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2.99) });
            }

            return token;
        }


        public void Dispose() {
            _client.Dispose();
        }

        #region kiosks
        /// <summary>
        /// Get kiosk
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task<Kiosk> GetKioskAsync(string uid) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_settings.Url), $"/api/kiosks?uid={uid}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());

            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Kiosk>(result);
        }

        public async Task<Kiosk> AddKioskAsync(string kioskUid) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), $"/api/kiosks?uid={kioskUid}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Kiosk>(result);
        }

        public async Task AddOrUpdateKioskAsync(Kiosk kiosk) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(_settings.Url), "/api/kiosks"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(kiosk, _options), Encoding.UTF8, "application/json");
            await _client.SendAsync(requestMessage);
        }

        public async Task<IEnumerable<Kiosk>> GetKiosksAsync() {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_settings.Url), "/api/kiosks/all"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Kiosk>>(result);
        }

        public async Task SetCreditAsync(string kioskUid, int credit) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), $"/api/kiosks/credit?kioskUid={kioskUid}&credit={credit}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        public async Task SetCreditAsync(string kioskUid, string sku, int credit) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), $"/api/kiosks/credit?kioskUid={kioskUid}&sku={sku}&credit={credit}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        public async Task SetMaxCountPerTransaction(string kioskUid, string sku, int maxCountPerTransaction) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), $"/api/kiosks/limit?kioskUid={kioskUid}&sku={sku}&limit={maxCountPerTransaction}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task KioskEnableAsync(string uid) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_settings.Url), $"/api/kiosks/enable?uid={uid}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Unique identifier of the kiosk</param>
        /// <returns></returns>
        public async Task KioskDisableAsync(string uid) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_settings.Url), $"/api/kiosks/disable?uid={uid}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        public async Task DispenseAsync(object address, string kioskUid) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), "/api/kiosks/dispense"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(new { address = address.ToString(), kioskUid }, _options), Encoding.UTF8, "application/json");
            await _client.SendAsync(requestMessage);
        }
        #endregion

        #region products
        public async Task<Product> GetProductAsync(string sku) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(_settings.Url), $"/api/ordering?sku={sku}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(result);
        }

        public async Task LinkProductAsync(string kioskUid, string sku) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(_settings.Url), $"/api/ordering/link?kioskUid={kioskUid}&sku={sku}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        public async Task UnlinkProductAsync(string kioskUid, string sku) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, new Uri(new Uri(_settings.Url), $"/api/ordering/unlink?kioskUid={kioskUid}&sku={sku}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            await _client.SendAsync(requestMessage);
        }

        public async Task DisableProductLinkAsync(string kioskUid, string sku) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_settings.Url), $"/api/ordering/link/disable?kioskUid={kioskUid}&sku={sku}"), null);
        }

        public async Task EnableProductLinkAsync(string kioskUid, string sku) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            await _client.PostAsync(new Uri(new Uri(_settings.Url), $"/api/ordering/link/enable?kioskUid={kioskUid}&sku={sku}"), null);
        }

        public async Task PutProductAsync(Product product) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_settings.Url), "/api/ordering"), httpContent);
        }

        public async IAsyncEnumerator<Product> GetProductsAsync(IEnumerable<string> sku, CancellationToken token) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(new ProductRequest { Sku = sku }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_settings.Url), $"/api/ordering/"), httpContent);

            response.EnsureSuccessStatusCode();

            using Stream responseStream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            var products = JsonSerializer.DeserializeAsyncEnumerable<Product>(
                responseStream,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 128
                });

            await foreach (var p in products) {
                if (token.IsCancellationRequested) {
                    responseStream.Close();
                    break;
                }
                else yield return p;
            }
        }

        public async IAsyncEnumerator<Product> GetProductsAsync(string filter, CancellationToken token) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            using HttpResponseMessage response = await _client.GetAsync(
                new Uri(new Uri(_settings.Url), $"/api/ordering/all?filter={filter ?? string.Empty}"),
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using Stream responseStream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
            var products = JsonSerializer.DeserializeAsyncEnumerable<Product>(
                responseStream,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 128
                });

            await foreach (var p in products) {
                if (token.IsCancellationRequested) {
                    responseStream.Close();
                    break;
                }
                else yield return p;
            }
        }

        public async Task<IEnumerable<Product>> DisableProductAsync(string kioskUid, string sku) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.DeleteAsync(new Uri(new Uri(_settings.Url), $"/api/ordering/disable"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(result);
        }

        public async Task PutPicture(string sku, string picture) {
            var httpContent = new StringContent(JsonSerializer.Serialize(new ProductPictureUpdateRequest { Sku = sku, Picture = picture }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_settings.Url), $"/api/ordering/picture"), httpContent);
            await response.Content.ReadAsStringAsync();
        }

        public async Task CommitTransactionsAsync(Transaction transaction) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(transaction, _options), Encoding.UTF8, "application/json");
            await _client.PutAsync(new Uri(new Uri(_settings.Url), "/api/ordering/transaction"), httpContent);
        }

        public async IAsyncEnumerator<Transaction> GetTransactionsAsync(TransactionRequest filter, CancellationToken? token = null) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _client.PostAsync(new Uri(new Uri(_settings.Url), "/api/ordering/transactions"), httpContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using Stream responseStream =
                token.HasValue ? await response.Content.ReadAsStreamAsync(token.Value).ConfigureAwait(false) :
                await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var transactions = JsonSerializer.DeserializeAsyncEnumerable<Transaction>(
                responseStream,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 128
                });

            await foreach (var s in transactions) {
                if (token != null && token.Value.IsCancellationRequested) {
                    responseStream.Close();
                    break;
                }
                else yield return s;
            }
        }
        #endregion

        #region login
        public async Task<HttpResponseMessage> LoginAsync(LoginRequest request) {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_settings.Url), "/api/customers/login"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await Token());
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await _client.SendAsync(requestMessage);
        }
        #endregion

        #region replenishment
        /// <summary>
        /// Kiosk Uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<PoG> GetPlanogramAsync(string uid) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_settings.Url), $"/api/replenishment/planogram?uid={uid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PoG>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">Kiosk Uid</param>
        /// <param name="planogram"></param>
        /// <returns></returns>
        public async Task PutPlanogramAsync(string uid, PoG planogram) {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var httpContent = new StringContent(JsonSerializer.Serialize(planogram), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_settings.Url), $"/api/replenishment/planogram?uid={uid}"), httpContent);
        }
        #endregion

        #region media
        public async Task<IEnumerable<AdMedia>> GetMediaAsync() {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_settings.Url), $"/api/media"));
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
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_settings.Url), $"/api/media/kiosk?uid={kioskUid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<KioskMediaLink>>(result);
        }

        public async Task PutMediaAsync(NewMediaRequest request) {
            var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_settings.Url), $"/api/media"), httpContent);
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

                var response = await _client.PostAsync(new Uri(new Uri(_settings.Url), $"/api/media/upload"), content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PutKioskMediaAsync(string kioskUid, IEnumerable<KioskMediaLink> resources) {
            var httpContent = new StringContent(JsonSerializer.Serialize(resources), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri(new Uri(_settings.Url), $"/api/kiosks/media/{kioskUid}"), httpContent);
        }

        public async Task<byte[]> DownloadMediaAsync(string hash, string format) {
            HttpResponseMessage response = await _client.GetAsync(new Uri(new Uri(_settings.Url), $"/api/media/find/{format}/{hash}"));
            Stream result = await response.Content.ReadAsStreamAsync();
            byte[] buffer = new byte[result.Length];
            await result.ReadAsync(buffer, 0, (int)result.Length);
            return buffer;
        }

        public async Task DeleteMediaAsync(string hash)
             => await _client.DeleteAsync(new Uri(new Uri(_settings.Url), $"/api/media/{hash}"));
        #endregion
    }
}