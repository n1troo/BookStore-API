using Blazored.LocalStorage;
using BookStore_UI_ServerSide.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Service
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public BaseRepository(IHttpClientFactory httpClient, 
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
    }
        public async Task<bool> Create(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (obj == null) { return false; }

            request.Content = new StringContent(JsonConvert.SerializeObject(obj));

            var client = _httpClient.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created) { return true; }

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1) { return false; }

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Content = JsonContent.Create(new { id });

            var client = _httpClient.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) { return true; }

            return false;

        }

        public async Task<T> Get(string url, int id)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _httpClient.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;

        }

        public async Task<IList<T>> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClient.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }

            return null;
        }

        public async Task<bool> Update(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            //request.Content = JsonContent.Create(new { obj });

            var client = _httpClient.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        private async Task<string> GetBearereToken()
        {
            return await _localStorageService.GetItemAsync<string>("authToken");
        }
    }
}
