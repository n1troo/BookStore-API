using Blazored.LocalStorage;
using BookStore_UI_ServerSide.Contracts;
using BookStore_UI_ServerSide.Models;
using BookStore_UI_ServerSide.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace BookStore_UI_ServerSide.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public async Task<bool> Register(RegistrationModel user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Static.Endpoints.RegisterEndpoint)
            {
                //request.Content = JsonContent.Create(new { user });
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage result = await client.SendAsync(request);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> Login(LoginModel user)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Static.Endpoints.LoginEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string content = await response.Content.ReadAsStringAsync();
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(content);

            //store token
            await _localStorageService.SetItemAsync("authToken", token.Token);

            //change auth state of app
           

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token.Token);


            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LogeOut();
        }

        public AuthenticationRepository(IHttpClientFactory httpClientFactory, 
            ILocalStorageService localStorageService, 
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }


    }
}
