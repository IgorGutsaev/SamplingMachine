using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MPT.SamplingMachine.ApiClient;
using MPT.Vending.API.Dto;

namespace Portal.StateContainers
{
    public class AppState
    {
        public User User { get; private set; }

        public bool Authorized => User != null;

        public AppState(SamplingMachineApiClient client, ILocalStorageService localStorage, NavigationManager uriHelper) {
            _client = client;
            _localStorage = localStorage;
            _uriHelper = uriHelper;
        }

        public async Task<bool> FetchUserAsync() {
            User user = await _localStorage.GetItemAsync<User>("user");
            if (user != null && !_client.TokenHaxExpired)
                User = user;

            return User != null;
        }

        public async Task LoginAsync(string email, string password) {
            _client.SetCredentials(email, password);

            User user = await _localStorage.GetItemAsync<User>("user");
            if (user != null && !_client.TokenHaxExpired)
                User = user;
            else {
                User = await _client.GetUserAsync();
                await _localStorage.SetItemAsync("user", User);
            }
        }

        public async Task SignUpAsync(string email, string password) {
            await _client.SignUpAsync(email, password);
            await LoginAsync(email, password);
        }

        public async Task LogoutAsync() {
            User = null;
            _uriHelper.NavigateTo("login");
            await _localStorage.RemoveItemAsync("user");
            _client.Logout();
        }

        private readonly SamplingMachineApiClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _uriHelper;
    }
}