using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MPT.SamplingMachine.ApiClient;
using MPT.Vending.API.Dto;

namespace Portal.StateContainers
{
    public class AppState
    {
        public User User { get; private set; }

        public AppState(SamplingMachineApiClient client, ILocalStorageService localStorage, NavigationManager uriHelper) {
            _client = client;
            _localStorage = localStorage;
            _uriHelper = uriHelper;
        }

        public async Task<bool> FetchUserAsync() {
            User user = await _localStorage.GetItemAsync<User>("user");
            if (user != null)
                User = user;

            return user != null;
        }

        public async Task LoginAsync(string email, string password) {
            _client.SetCredentials(email, password);

            User user = await _localStorage.GetItemAsync<User>("user");
            if (user != null)
                User = user;
            else {
                User = await _client.GetUserAsync();
                await _localStorage.SetItemAsync("user", User);
            }
        }


        //public async Task SignUp(SignUpDetails signUpData) {
        //    var response = await _httpClient.PostAsync(Urls.Signup, new StringContent(System.Text.Json.JsonSerializer.Serialize(signUpData), Encoding.UTF8, "application/json"));
        //    if (response.IsSuccessStatusCode) {
        //        await Login(new LoginDetails { Username = signUpData.Username, Password = signUpData.Password });
        //    }
        //    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //        throw new ArgumentException(response.Content.ReadAsStringAsync().Result);
        //}

        //public async Task PutUser(User user) {
        //    var response = await _httpClient.PostAsync(Urls.PutUser, new StringContent(System.Text.Json.JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));
        //}

        //public async Task Logout() {
        //    await _localStorage.RemoveItemAsync("authToken");
        //    IsLoggedIn = false;
        //    OnLogInOut?.Invoke(this, false);
        //}

        //private async Task SaveToken(HttpResponseMessage response) {
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    var jwt = Helpers.SerializationHelper.DeserializeJson<JwtToken>(responseContent);
        //    await _localStorage.SetItemAsync("authToken", jwt.Token);
        //}

        //private async Task SetAuthorizationHeader() {
        //    var token = await _localStorage.GetItemAsync<string>("authToken");

        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //}

        ///// <summary>
        ///// Try get auth token from local storage. It will be convinient if user open page with direct link (for example in new browser page)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="action"></param>
        ///// <returns></returns>
        //public async Task<T> Request<T>(HttpClient client, Func<HttpClient, Task<T>> action) {
        //    if (client.DefaultRequestHeaders.Authorization == null) {
        //        await SetAuthorizationHeader();
        //        var token = await _localStorage.GetItemAsync<string>("authToken");
        //        if (string.IsNullOrWhiteSpace(token))
        //            IsLoggedIn = true;
        //        else Console.WriteLine("something wrong!");
        //    }
        //    else IsLoggedIn = true;

        //    return await action(client);
        //}

        private readonly SamplingMachineApiClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _uriHelper;
    }
}
