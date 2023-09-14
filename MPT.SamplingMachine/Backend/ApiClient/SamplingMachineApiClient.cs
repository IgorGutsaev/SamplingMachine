using MPT.Vending.API.Dto;
using System.Text.Json;

namespace MPT.SamplingMachine.ApiClient
{
    public class SamplingMachineApiClient
    {
        private readonly string _url;

        public SamplingMachineApiClient(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            _url = url;
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
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks?uid={uid}"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<KioskDto>(result);
        }

        public async Task<IEnumerable<KioskDto>> GetKiosksAsync()
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(new Uri(_url), $"/api/kiosks/all"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<KioskDto>>(result);
        }
        #endregion

        #region products
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            // request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(new Uri(_url), $"/api/products/all"));
            string result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(result);
        }
        #endregion
    }
}